import 'package:flutter/material.dart';
import 'package:flutter_form_builder/flutter_form_builder.dart';
import 'package:form_builder_validators/form_builder_validators.dart';
import 'package:formulario_opret/models/formulario_Registro.dart';
import 'package:formulario_opret/screens/interfaz_User/navbarUser/navbar_Empl.dart';
import 'package:formulario_opret/screens/interfaz_User/pregunta_Encuesta_Screen.dart';
import 'package:formulario_opret/services/form_Registro_services.dart';
import 'package:formulario_opret/widgets/input_decoration.dart';
import 'package:intl/intl.dart';

class FormEncuestaScreen extends StatefulWidget {
  final TextEditingController filtrarUsuarioController;
  final TextEditingController filtrarEmailController;
  final TextEditingController filtrarId;
  final TextEditingController filtrarCedula;

  const FormEncuestaScreen({
    super.key,
    required this.filtrarId,
    required this.filtrarCedula,
    required this.filtrarUsuarioController,
    required this.filtrarEmailController
  });

  @override
  State<FormEncuestaScreen> createState() => _FormEncuestaScreenState();
}

class _FormEncuestaScreenState extends State<FormEncuestaScreen> {
  final _formKey = GlobalKey<FormBuilderState>();
  final ApiServiceFormRegistro _apiServiceFormRegistro = ApiServiceFormRegistro('https://10.0.2.2:7002');
  final TextEditingController timePicker = TextEditingController();
  final TextEditingController datePicker = TextEditingController();
  final TextEditingController noEncuestaFiltrar = TextEditingController();
  DateTime? _selectedDate;  // Variable para almacenar la fecha seleccionada.

  Future<void> _showDatePicker() async {
    final picked = await showDatePicker(
      context: context,
      initialDate: DateTime.now(), 
      firstDate: DateTime(2024, 9, 1), 
      lastDate: DateTime.now(),
      builder: (BuildContext content, Widget? child) {
        return Theme(
          data: ThemeData.light().copyWith(
            colorScheme: const ColorScheme.light(primary: Colors.green),
            buttonTheme: const ButtonThemeData(textTheme: ButtonTextTheme.primary),
          ),
          child: child!,
        );
      }
    );

    if (picked != null && picked != _selectedDate) {
      setState(() {
        _selectedDate = picked;
        datePicker.text = DateFormat("yyyy-MM-dd").format(_selectedDate!); // Formatea la fecha seleccionada
      });
    }
  }

  void _registrarFormEncuesta() async {
    if (_formKey.currentState!.saveAndValidate()) {
      final data = _formKey.currentState!.value;

      final formEncuesta = FormularioRegistro(
        noEncuesta: data['noEncuesta'],
        // idUsuarioEmpl: widget.filtrarId.text,
        idUsuarioEmpl: data['idUsuarioEmpl'],
        // cedlEmpleado: widget.filtrarCedula.text,
        cedlEmpleado: data['cedlEmpleado'],
        fechaEncuesta: datePicker.text, // Utiliza la fecha seleccionada
        hora: data['hora'],
        estacion: data['estacion'],
        linea: data['linea']
      );

      // Imprimir los datos a enviar para depuración
      print('Datos del formulario: ${formEncuesta.toJson()}');

      try{
        final response = await _apiServiceFormRegistro.postFormRegistro(formEncuesta);

        if(response.statusCode == 201){
          ScaffoldMessenger.of(context).showSnackBar(
            const SnackBar(content: Text('Formulario enviado con exito'))
          );

          Navigator.push(
            context,
            MaterialPageRoute(
              builder: (context) => PreguntaEncuestaScreen(
                noEncuestaFiltrar: noEncuestaFiltrar, //para filtrarlo a la pantalla de "PreguntaEncuestaScreen"
                filtrarUsuarioController: widget.filtrarUsuarioController,  
                filtrarEmailController: widget.filtrarEmailController,
                filtrarId: widget.filtrarId,
                filtrarCedula: widget.filtrarCedula,
              )
            ),
          );

        } else {
          // Mostrar mensaje de error
          ScaffoldMessenger.of(context).showSnackBar(
            SnackBar(content: Text('Error al enviar formulario: ${response.reasonPhrase}')),
          );
        }

      } catch (e) {
        // Manejo de errores
        ScaffoldMessenger.of(context).showSnackBar(
          SnackBar(content: Text('Error: $e')),
        );
      }
    }
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      drawer: NavbarEmpl(
        filtrarUsuarioController: widget.filtrarUsuarioController,  
        filtrarEmailController: widget.filtrarEmailController,
        filtrarId: widget.filtrarId,
        filtrarCedula: widget.filtrarCedula,
      ),

      appBar: AppBar(title: const Text('Formulario')),

      body: SingleChildScrollView(
        child: FormBuilder(
          key: _formKey,
          child: Column(
            mainAxisSize: MainAxisSize.min,
            children: [
              FormBuilderTextField(
                name: 'noEncuesta',
                controller: noEncuestaFiltrar,
                decoration: InputDecorations.inputDecoration(
                  hintext: '#',
                  hintFrontSize: 25.0,
                  labeltext: 'No. de Encuesta',
                  labelFrontSize: 30.5,
                  icono: const Icon(Icons.numbers, size: 30.0)
                ),
                style: const TextStyle(fontSize: 23.5), // Cambiar tamaño de letra del texto filtrado
                validator: FormBuilderValidators.required(),
                onChanged: (val) {
                  print('Numero seleccionada: $val');
                },
              ),

              FormBuilderTextField(
                name: 'idUsuarioEmpl',
                initialValue: widget.filtrarId.text,
                decoration: InputDecorations.inputDecoration(
                  labeltext: 'Asignar ID',
                  labelFrontSize: 25.5, // Tamaño de letra personalizado
                  hintext: 'USER-000000000',
                  hintFrontSize: 20.0,
                  icono: const Icon(Icons.perm_identity_outlined,size: 30.0),
                ),
                // validator: FormBuilderValidators.required(),
                validator: (value) {
                  if (value == null || value.isEmpty){
                    return 'Por favor ingrese su ID-Empleado';
                  }

                  if (!RegExp(r'^USER-\d{4,10}$').hasMatch(value)){
                    return 'Por favor ingrese un ID-Empleado valido';
                  }

                  return null;
                },
                onChanged: (val) {
                  print('Id seleccionada: $val');
                },
              ),

              FormBuilderTextField(
                name: 'cedlEmpleado',
                initialValue: widget.filtrarCedula.text,
                decoration: InputDecorations.inputDecoration(
                  labeltext: 'Cedula',
                  labelFrontSize: 25.5,
                  hintext: '000-0000000-0',
                  hintFrontSize: 20.0, 
                  icono: const Icon(Icons.person_pin_circle_outlined, size: 30.0),
                ),
                // validator: FormBuilderValidators.required(),
                validator: FormBuilderValidators.compose([ //Combina varios validadores. En este caso, se utiliza el validador requerido y una función personalizada para la expresión regular.
                  FormBuilderValidators.required(errorText: 'Debe de ingresar la cedula'), //Valida que el campo no esté vacío y muestra el mensaje 'El correo es obligatorio' si no se ingresa ningún valor.
                  (value) {
                    // Expresión regular para validar la cedula
                    String pattern = r'^\d{3}-\d{7}-\d{1}$';
                    RegExp regExp = RegExp(pattern);
                    // if(value == null || value.isEmpty){
                    //   return 'Por favor ingrese su cédula';
                    // }

                    if(!regExp.hasMatch(value ?? '')){
                      return 'Formato de cédula incorrecto';
                    }
                    return null;
                  },
                ]),
                onChanged: (val) {
                  print('Cedula seleccionada: $val');
                },                 
              ),

              FormBuilderTextField(
                name: 'hora',
                controller: timePicker,
                decoration: const InputDecoration(
                  // hintext: 'Fecha actual',
                  // hintFrontSize: 20.0, 
                  labelText: 'Hora de Encuesta',
                  labelStyle: TextStyle(fontSize: 25.0),
                  prefixIcon: Icon(Icons.access_time, size: 30.0)
                ),
                onTap: () async {
                  var time = await showTimePicker(
                    context: context, 
                    initialTime: TimeOfDay.now()
                  );

                  if (time != null) {
                    setState(() {
                      timePicker.text = time.format(context);
                    });
                  }
                },
                onChanged: (val) {
                  print('Hora seleccionada: $val');
                },
              ),

              FormBuilderTextField(
                name: 'fechaEncuesta',
                controller: datePicker,
                decoration: const InputDecoration(
                  // hintext: 'Hora actual',
                  // hintFrontSize: 20.0,
                  labelText: 'Fecha de Encuesta',
                  labelStyle: TextStyle(fontSize: 25.0),
                  prefixIcon: Icon(Icons.calendar_month_outlined, size: 30.0)
                ),
                validator: FormBuilderValidators.required(),
                onTap: () async {
                  FocusScope.of(context).requestFocus(FocusNode()); // Cierra el teclado al hacer clic
                  await _showDatePicker(); // Muestra el DatePicker
                },
              ),

              FormBuilderTextField(
                name: 'linea',
                decoration: InputDecorations.inputDecoration(
                  hintext: 'Linea 1, 2 ... o Teleferico',
                  hintFrontSize: 25.0,
                  labeltext: 'Linea del metro',
                  labelFrontSize: 30.5,
                  icono: const Icon(Icons.numbers, size: 30.0)
                ),
                style: const TextStyle(fontSize: 23.5), // Cambiar tamaño de letra del texto filtrado
                validator: FormBuilderValidators.required(),
              ),

              FormBuilderTextField(
                name: 'estacion',
                decoration: InputDecorations.inputDecoration(
                  hintext: 'Estacion en la que se encuentre el Encuestador',
                  hintFrontSize: 25.0,
                  labeltext: 'Estacion del metro',
                  labelFrontSize: 30.5,
                  icono: const Icon(Icons.numbers, size: 30.0)
                ),
                style: const TextStyle(fontSize: 23.5), // Cambiar tamaño de letra del texto filtrado
                maxLines: 5, // Permite hasta 5 líneas de texto
                validator: FormBuilderValidators.required(),
              ),

              ElevatedButton(
                onPressed: _registrarFormEncuesta, 
                
                style: ElevatedButton.styleFrom(
                  backgroundColor: const Color.fromRGBO(1, 135, 76, 1), //  se usa para definir el color de fondo del botón.
                  foregroundColor: const Color.fromARGB(255, 255, 255, 255), // se usa para definir el color del texto y los iconos dentro del botón.
                  padding: const EdgeInsets.symmetric(horizontal: 40, vertical: 15),
                  shape: RoundedRectangleBorder(
                    borderRadius: BorderRadius.circular(50),
                  ),
                ),

                child: const Text(
                  'Inicial Encuesta',
                  style: TextStyle(
                    fontSize: 20, 
                    fontWeight: FontWeight.bold
                  )
                ),
              ),
            ],
          ),
        ),
      ),
    );
  }
}

// var _currentSelecterDate;

  // void callDatePicker() async {
  //   var selectDate = await getDataPicker();
  //   setState(() {
  //     _currentSelecterDate = selectDate;
  //   });
  // }

  // Future<DateTime?> getDataPicker() {
  //   return showDatePicker(
  //     context: context,
  //     initialDate: DateTime.now(),
  //     firstDate: DateTime(2024), 
  //     lastDate: DateTime(2200), // Fecha máxima permitida
  //     builder: (context, child) {
  //       return Theme(data: ThemeData.dark(), child: child);
  //     }
  //   );
  // }

  
  // FormBuilderDateTimePicker(
  //   name: 'fechaNacimiento',
  //   // initialValue: DateTime.now(),
  //   // initialEntryMode: DatePickerEntryMode.calendar,
  //   inputType: InputType.date,
  //   format: DateFormat('yyyy-MM-dd'), // Define el formato de fecha
  //   decoration: InputDecorations.inputDecoration(
  //     hintext: 'Fecha actual',
  //     hintFrontSize: 20.0, 
  //     labeltext: 'Fecha de Encuesta',
  //     labelFrontSize: 25.5,
  //     icono: const Icon(Icons.calendar_month_outlined, size: 30.0)
  //   ),
  //   initialDate: DateTime.now(), // Puedes establecer una fecha inicial
  //   validator: FormBuilderValidators.compose([
  //     FormBuilderValidators.required(errorText: 'Por favor seleccione una fecha'),
  //   ]),
  // ),

  // FormBuilderDateTimePicker(
  //   name: 'hora',
  //   initialEntryMode: DatePickerEntryMode.calendar,
  //   inputType: InputType.time,
  //   decoration: InputDecorations.inputDecoration(
  //     hintext: 'Hora actual',
  //     hintFrontSize: 20.0,
  //     labeltext: 'Hora de Encuesta',
  //     labelFrontSize: 25.5,
  //     icono: const Icon(Icons.access_time, size: 30.0),
  //   ),
  //   initialTime: TimeOfDay.now(), // Establece una hora inicial
  //   validator: FormBuilderValidators.compose([
  //     FormBuilderValidators.required(errorText: 'Por favor seleccione una hora'),
  //   ]),
  // ),

  // // Convertir TimeOfDay a String en formato 24 horas con segundos
  // final TimeOfDay? time = data['hora'];
  // String? formattedTime;
  // if (time != null) {
  //   final now = DateTime.now();
  //   formattedTime = '${time.hour.toString().padLeft(2, '0')}:${time.minute.toString().padLeft(2, '0')}:${now.second.toString().padLeft(2, '0')}:00';
  // }

  // // Convertir DateTime a solo fecha (DateOnly)
  // final DateTime? fecha = data['fechaEncuesta'];
  // String? formattedDate;
  // if (fecha != null) {
  //   formattedDate = DateFormat('yyyy-MM-dd').format(fecha); // Formato 'DateOnly'
  // }

  // FormBuilderTextField(
  //   name: 'fechaEncuesta',
  //   controller: datePicker,
  //   decoration: const InputDecoration(
  //     // hintext: 'Hora actual',
  //     // hintFrontSize: 20.0,
  //     labelText: 'Fecha de Encuesta',
  //     labelStyle: TextStyle(fontSize: 25.0),
  //     prefixIcon: Icon(Icons.calendar_month_outlined, size: 30.0)
  //   ),
  //   onTap: () async {
  //     DateTime? dateTime = await showDatePicker(
  //       context: context, 
  //       initialDate: DateTime.now(),
  //       firstDate: DateTime(2024), 
  //       lastDate: DateTime(2200),
  //     );

  //     if (dateTime != null) {
  //       String formattedDate = DateFormat('yyyy-MM-dd').format(dateTime);

  //       setState(() {
  //         datePicker.text = formattedDate;
  //       });
  //     }
  //   },
  // ),

  // FormBuilderDateTimePicker(
  //   name: 'fechaNacimiento',
  //   // initialValue: DateTime.now(),
  //   // initialEntryMode: DatePickerEntryMode.calendar,
  //   inputType: InputType.date,
  //   format: DateFormat('yyyy-MM-dd'), // Define el formato de fecha
  //   decoration: InputDecorations.inputDecoration(
  //     hintext: 'Fecha actual',
  //     hintFrontSize: 20.0, 
  //     labeltext: 'Fecha de Encuesta',
  //     labelFrontSize: 25.5,
  //     icono: const Icon(Icons.calendar_month_outlined, size: 30.0)
  //   ),
  //   initialDate: DateTime.now(), // Puedes establecer una fecha inicial
  //   firstDate: DateTime(2024),
  //   lastDate: DateTime(2200), 
  //   validator: FormBuilderValidators.compose([
  //     FormBuilderValidators.required(errorText: 'Por favor seleccione una fecha'),
  //   ]),
  //   onChanged: (val) {
  //     print('Fecha seleccionada: $val');
  //   },
  // ),