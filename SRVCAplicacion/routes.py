from klappy import db,app
from marshmallow import ValidationError
from flask import jsonify,request
from models import PuntoDeControl,Solicitud,Pass
from schemas import solicitud_Schema, punto_control_Schema
import secrets
import string
from task import generar_token_seguro
from task import cifrar_mensaje, descifrar_mensaje,obtener_datos_punto_control_json
from dbbasepuntocontrol import DB_estructura



# import logging

# # Configuración del logger para rutas
# logging.basicConfig(
#     filename='routes.log',  # Nombre del archivo de log
#     filemode='a',           # Modo de apertura del archivo ('a' para añadir)
#     format='%(asctime)s - %(name)s - %(levelname)s - %(message)s',  # Formato de los mensajes
#     level=logging.INFO      # Cambia a DEBUG para más información si es necesario
# )

# logger2 = logging.getLogger('routes')  # Nombre del logger para rutas

@app.route('/api/data')
def get_data():
    datos = PuntoDeControl.query.all()  # Consultar todos los registros de puntos de control
    return jsonify([dato.to_dict() for dato in datos])  # Convertir a JSON
   
    
@app.route('/api/actualizar_solicitud/<int:id>', methods=['PUT'])  # Cambiamos a PUT para actualizar
def actualizar_solicitud(id):
    # Obtener el nuevo estado desde la solicitud
    estado_nuevo = request.json.get('estado')

    # Verificar que se haya pasado un estado
    if estado_nuevo is None:
        return jsonify({'error': 'Falta el estado en la solicitud.'}), 400

    # Validar que el estado sea uno de los permitidos
    if estado_nuevo not in [0, 1]:
        return jsonify({'error': 'Estado inválido, debe ser "aceptado" o "rechazado".'}), 400

    # Buscar la solicitud por su ID
    solicitud = Solicitud.query.get(id)

    # Verificar si la solicitud existe
    if not solicitud:
        return jsonify({'error': 'Solicitud no encontrada.'}), 404

    # Actualizar el estado de la solicitud
    solicitud.estado = estado_nuevo

    # Guardar los cambios en la base de datos
    try:
        db.session.commit()
        return jsonify({'message': f'Estado Solicitud: {estado_nuevo}'}), 200
    except Exception as e:
        db.session.rollback()
        return jsonify({'error': str(e)}), 500
    
@app.route('/api/obtener_solicitud/<int:id>', methods=['GET'])  
def obtener_estado_solicitud(id):
    # Buscar la solicitud por su ID
    solicitud = Solicitud.query.get(id)
    # Verificar si la solicitud existe
    if not solicitud:
        return jsonify({'error': 'Solicitud no encontrada.'}), 404
    # Devolver el estado actual de la solicitud
    return jsonify({'estado': solicitud.estado}), 200



@app.route('/api/nueva_solicitud', methods=['POST'])
def nueva_solicitud():
    # Instancia del esquema
    schema = solicitud_Schema()

    # Validar y deserializar los datos de la solicitud
    try:
        # Cargamos y validamos los datos usando el esquema
        data = schema.load(request.json)
    except ValidationError as err:
        return jsonify({'errors': err.messages}), 400

    # Obtener la IP del cliente que hace la solicitud
    ip = request.remote_addr

    # Crear una nueva solicitud
    nueva_solicitud = Solicitud(
        nombre=data['nombre'],
        dni=data['dni'],
        ip=ip,
        tipo=data['tipo'],
        estado=2  # Asignar un valor por defecto para 'estado'
    )

    # Guardar la nueva solicitud en la base de datos
    try:
        db.session.add(nueva_solicitud)
        db.session.commit()
        return jsonify({
            'message': 'Solicitud creada exitosamente, por favor espere su procesamiento:',
            'id_solicitud': nueva_solicitud.id_solicitudes
        }), 201
    except Exception as e:
        db.session.rollback()
        return jsonify({'error': str(e)}), 500

@app.route('/api/aceptar_solicitud_nuevo_punto', methods=['POST'])
def aceptar_solicitud_nuevo_punto():
    schema_punto_control = punto_control_Schema()
    schema_solicitud = solicitud_Schema()
    data = request.json

    try:
        # Validar datos de solicitud y obtener ID
        solicitud_data = schema_solicitud.load({'id_solicitud': data.get('id_solicitud')})
        id_solicitud = solicitud_data['id_solicitud']

        # Validar nombre del punto de control
        punto_data = schema_punto_control.load({'nombre_punto_control': data.get('nombre_punto_control')})
        nombre_punto_control = punto_data['nombre_punto_control']

        # Obtener la solicitud por su id
        solicitud = Solicitud.query.get(id_solicitud)
        if not solicitud:
            return jsonify({'error': 'Solicitud no encontrada.'}), 404

        # Verificar si la solicitud ya tiene el estado que queremos (estado 1)
        if solicitud.estado == 1:
            return jsonify({'error': 'La solicitud ya está en estado 1.'}), 400

        # Verificar si la solicitud es para un nuevo punto (tipo 1)
        if solicitud.tipo != 1:
            return jsonify({'error': 'La solicitud no es para dar de alta un nuevo puento.'}), 400


        # Generar tokens y crear el nuevo PuntoDeControl
        token_punto = secrets.token_hex(16)
        seed = secrets.token_hex(32)
        token_temporal_generado = generar_token_seguro()

        nuevo_punto = PuntoDeControl(
            token=token_punto,
            nombre_punto_control=nombre_punto_control,
            estado=1,
            token_temporal=token_temporal_generado
        )

        # Añadir el nuevo PuntoDeControl y hacer commit para obtener su ID
        db.session.add(nuevo_punto)
        db.session.flush()  # Fuerza a que los cambios sean reflejados
        db.session.commit()

        # Asignar id_punto_control y cambiar estado de la solicitud
        solicitud.id_punto_control = nuevo_punto.id_punto_control
        solicitud.estado = 1  # Cambiar el estado a 1

        # Crear nuevo Pass asociado al nuevo PuntoDeControl
        nuevo_pss = Pass(
            id_punto=nuevo_punto.id_punto_control,
            s=seed
        )

        # Añadir la solicitud y Pass a la sesión y hacer commit final
        db.session.add(solicitud)
        db.session.add(nuevo_pss)
        db.session.commit()

        return jsonify({
            'message': 'Solicitud aceptada y punto de control creado.',
            'estado Solicitud': solicitud.estado,
            'id_punto_control': nuevo_punto.id_punto_control,
            'token': token_temporal_generado
        }), 200

    except Exception as e:
        db.session.rollback()
        return jsonify({'error': 'Error al actualizar la solicitud y crear el punto de control: ' + str(e)}), 500

@app.route('/api/tomar_responder_sol_nuevo_punto', methods=['POST'])
#Tomamos el token temporal desde el lado del cliente y le damos la base de datos del punto de control
def tomar_responder_sol_nuevo_punto():

    # schema
    schema_punto_control = punto_control_Schema()
    schema_solicitud = solicitud_Schema()
    # Obtener y validar los datos del JSON entrante
    data = request.json  # Asegúrate de que 'data' esté definido con el JSON de la solicitud

    try:
        solicitud_data = schema_solicitud.load({'id_solicitud': data.get('id_solicitud')})
        id_solicitud = solicitud_data['id_solicitud']
    except ValidationError as err:
        return jsonify({'error': 'Datos de la solicitud inválidos', 'detalles': err.messages}), 400

    # Validar y extraer el nombre del punto de control (usando schema_punto_control)
    try:
        punto_data = schema_punto_control.load({'token_temporal': data.get('token_temporal')})
        token_temporal_ingresado = punto_data['token_temporal']
    except ValidationError as err:
        return jsonify({'error': 'Datos del punto de control inválidos', 'detalles': err.messages}), 400

    # Obtener la solicitud por su id
    solicitud = Solicitud.query.get(id_solicitud)
# Verificar si la solicitud fue encontrada y si su estado es igual a 1
    if solicitud is None:
        return jsonify({"error": "La solicitud no existe."}), 404   
    if  solicitud.estado != 1:
            return jsonify({"message": "El estado de la solicitud no es 1."}), 200    
   
    punto_control = PuntoDeControl.query.get(solicitud.id_punto_control)
    id_del_punto_control = punto_control.id_punto_control
    token_temporal_punto = punto_control.token_temporal

# validar token temporal
    if token_temporal_ingresado != token_temporal_punto:
        return jsonify({"error": "Token incorrecto."}), 404 

    datos_punto_control = obtener_datos_punto_control_json(id_del_punto_control)
    punto_control.token_temporal = ""
    db.session.add(punto_control)
    db.session.commit()
    return [datos_punto_control,DB_estructura], 200

    
@app.route('/api/punto_control/<int:id>', methods=['GET'])
def obtener_punto_control(id):
    try:
        # Buscar el punto de control por su ID
        punto_control = PuntoDeControl.query.get(id)
        
        # Verificar si el punto de control existe
        if not punto_control:
            return jsonify({'error': 'Punto de control no encontrado.'}), 404

        # Convertir el resultado en un diccionario JSON
        punto_control_json = {
            "id_punto_control": punto_control.id_punto_control,
            "token": punto_control.token,
            "nombre_punto_control": punto_control.nombre_punto_control,
            "estado": punto_control.estado,
            # Relaciones, si se desean incluir:
            "logs_aud": [log.id for log in punto_control.Log_Aud_punto_control],
            "motivos": [motivo.id for motivo in punto_control.Motivo_punto_control],
            "registro_visitas": [registro.id for registro in punto_control.RegistroVisitas_punto_control],
            "usuarios": [usuario.id for usuario in punto_control.Usuario_punto_control],
            "visitantes_inquilinos": [visitante.id for visitante in punto_control.VisitanteInquilino_punto_control]
                }        
        return jsonify(punto_control_json,DB_estructura), 200
    except Exception as e:
        return jsonify({'error': 'Error al obtener el punto de control', 'detalle': str(e)}), 500
    
@app.route('/api/armartoken', methods=['GET'])
def armartoken():
    token=generar_token_seguro()
    token_punto = secrets.token_hex(16)
    return [token,token_punto]

@app.route('/api/aceptar_solicitud_tranferir_punto', methods=['POST'])
def aceptar_solicitud_transferir_punto():

    schema_solicitud = solicitud_Schema()
    data = request.json

    try:
        # Validar datos de solicitud y obtener ID
        solicitud_data = schema_solicitud.load({'id_solicitud': data.get('id_solicitud')})
        id_solicitud = solicitud_data['id_solicitud']

        # Obtener la solicitud por su id
        solicitud = Solicitud.query.get(id_solicitud)
        if not solicitud:
            return jsonify({'error': 'Solicitud no encontrada.'}), 404

        # Verificar si la solicitud ya tiene el estado que queremos (estado 1)
        if solicitud.estado == 1:
            return jsonify({'error': 'La solicitud ya está en estado 1.'}), 400

        # Verificar si la solicitud es para transferir un punto (tipo 2)
        if solicitud.tipo != 2:
            return jsonify({'error': 'La solicitud no es para tranferir un punto.'}), 400

        # Buscar el punto de control por su ID
        punto_control = PuntoDeControl.query.get(solicitud.id_punto_control)
        if not punto_control:
            return jsonify({'error': 'Punto de control no encontrado. ID buscado:'&solicitud.id_punto_control}), 404

        # Generar tokens y crear el nuevo PuntoDeControl
        token_punto = secrets.token_hex(16)
        seed = secrets.token_hex(32)
        token_temporal_generado = generar_token_seguro()

        #Asignar nuevos valores al punto de contorl existente 
        punto_control.token_temporal = token_temporal_generado
        punto_control.token = token_punto
        punto_control.estado = 1
        id_punto_control = punto_control.id_punto_control

        # Añadir el nuevo PuntoDeControl y hacer commit para obtener su ID
        db.session.add(punto_control)
        db.session.flush()  # Fuerza a que los cambios sean reflejados
        db.session.commit()
        solicitud.estado = 1  # Cambiar el estado a 1

        # Crear nuevo Pass asociado al nuevo PuntoDeControl
        registro_pass = Pass.query.filter_by(id_punto=id_punto_control).first()
        registro_pass.seed = seed

        # Añadir la solicitud y Pass a la sesión y hacer commit final
        db.session.add(solicitud)
        db.session.add(registro_pass)
        db.session.commit()

        return jsonify({
            'message': 'Solicitud aceptada y punto actualizado.',
            'estado Solicitud': solicitud.estado,
            'id_punto_control':id_punto_control,
            'token': token_temporal_generado
        }), 200

    except Exception as e:
        db.session.rollback()
        return jsonify({'error': 'Error al actualizar la solicitud y actualizar punto de control: ' + str(e)}), 500