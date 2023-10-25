using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection.PortableExecutable;

class Ecuacion
{
    // Función para validar que la ecuación no comience ni termine con algún operador
    static void ValidarOperadoresOperandos(string input)
    {
        List<char> operandos = new List<char> { '+', '-', '/', '*', '.' };

        char primerCaracter = input[0];
        char ultimoCaracter = input[input.Length - 1];

        if (operandos.Contains(primerCaracter) || operandos.Contains(ultimoCaracter))
        {
            if (primerCaracter != '-')
            {
                throw new InvalidOperationException(
                    "El primer y último carácter de la ecuación no pueden ser operadores o puntos decimales."
                );
            }
        }
    }

    // Función para verificar que no haya dos puntos decimales consecutivos
    static void ValidarPuntosDecimales(string input)
    {
        bool puntoDecimalEnCurso = false;

        for (int i = 0; i < input.Length; i++)
        {
            char caracterActual = input[i];

            if (caracterActual == '.')
            {
                if (puntoDecimalEnCurso)
                {
                    throw new InvalidOperationException(
                        "Dos puntos decimales consecutivos no están permitidos."
                    );
                }
                puntoDecimalEnCurso = true;
            }
            else if (!Char.IsDigit(caracterActual))
            {
                puntoDecimalEnCurso = false;
            }

            if (i == input.Length - 1 && caracterActual == '.')
            {
                throw new InvalidOperationException(
                    "El número no puede terminar con un punto decimal."
                );
            }
        }
    }

    static void ValidarParentesis(string input)
    {
        int contadorParentesis = 0;

        for (int i = 0; i < input.Length; i++)
        {
            char caracterActual = input[i];
            char caracAnterior = (i > 0) ? input[i - 1] : ' ';
            char caracSig = (i < input.Length - 1) ? input[i + 1] : ' ';

            if (caracterActual == '(')
            {
                contadorParentesis++;
                if (caracSig == '+' || caracSig == '*' || caracSig == '/' || caracSig == '.')
                {
                    throw new InvalidOperationException(
                        "Operador no válido después de abrir un paréntesis."
                    );
                }
                if (caracAnterior == '.')
                {
                    throw new InvalidOperationException(
                        "Previo a un paréntesis no puede haber un '.'."
                    );
                }
                if( caracSig == ')'){
                    throw new InvalidOperationException(
                        "Los parentesis no pueden estar vacios"
                    );
                }
            }
            else if (caracterActual == ')')
            {
                contadorParentesis--;
                if (contadorParentesis < 0)
                {
                    throw new InvalidOperationException("Los paréntesis no están balanceados.");
                }
                if (
                    caracAnterior == '+'
                    || caracAnterior == '-'
                    || caracAnterior == '*'
                    || caracAnterior == '/'
                    || caracAnterior == '.'
                )
                {
                    throw new InvalidOperationException(
                        "Operador no válido antes de cerrar un paréntesis."
                    );
                }
            }
        }

        if (contadorParentesis != 0)
        {
            throw new InvalidOperationException("Los paréntesis no están balanceados.");
        }
    }

    // Función para verificar que no haya una división por cero DIRECTA → 2/0
    static void ValidarDivisionPorCero(string input)
    {
        for (int i = 0; i < input.Length - 1; i++)
        {
            char caracterActual = input[i];
            char caracSig = input[i + 1];

            if(caracterActual == '/' && input[input.Length - 1] == '0'){
                throw new InvalidOperationException("División por cero detectada.");
            }
            else if (caracterActual == '/' && caracSig == '0')
            {
                if (input[i + 2] == '.' && !Char.IsDigit(input[i + 3]))
                {
                    throw new InvalidOperationException("Expresión incorrecta.");
                }
                else if (input[i + 2] != '.')
                {
                    throw new InvalidOperationException("División por cero detectada.");
                }
            }
        }
    }

    // Función para verificar que no haya caracteres inválidos
    static void ValidarCaracteres(string input)
    {
        List<char> caracteresValidos = new List<char>
        {
            '+',
            '-',
            '/',
            '*',
            '(',
            ')',
            '0',
            '1',
            '2',
            '3',
            '4',
            '5',
            '6',
            '7',
            '8',
            '9',
            '.'
        };

        foreach (char caracter in input)
        {
            if (!caracteresValidos.Contains(caracter))
            {
                char auxChar = char.ToLower(caracter);
                switch (auxChar)
                {
                    case ',':
                        throw new InvalidOperationException(
                            "Utilizó un carácter inválido: '"
                                + caracter
                                + "'. Prefiera el uso de '.' en lugar de este carácter."
                        );
                    case ' ':
                        throw new InvalidOperationException(
                            "Utilizó un carácter inválido: ' '. Escriba la ecuación sin espacios."
                        );
                    case 'x':
                        throw new InvalidOperationException(
                            "Utilizó un carácter inválido: '"
                                + caracter
                                + "'. Prefiera el uso de '*' en lugar de este carácter."
                        );
                    default:
                        throw new InvalidOperationException(
                            "Utilizó un carácter inválido: " + caracter
                        );
                }
            }
        }
    }

    // Función para chequear que no haya signos repetidos + - * / .
    static void ValidarOperandosRepetidos(string input)
    {
        List<char> operandos = new List<char> { '+', '-', '/', '*', '.' };

        for (int i = 0; i < input.Length - 1; i++)
        {
            char caracterActual = input[i];
            char caracSig = input[i + 1];

            if (operandos.Contains(caracterActual) && operandos.Contains(caracSig))
            {
                throw new InvalidOperationException("No pueden haber dos operandos consecutivos.");
            }
        }
    }

    static void ValidarOperandoMinimo(string input)
    {
        List<char> operandos = new List<char> { '+', '-', '*', '/' };
        bool contieneOperando = false;

        if (input[0] == '-')
        {
            for (int i = 1; i < input.Length; i++)
            {
                char caracter = input[i];
                if (operandos.Contains(caracter))
                {
                    contieneOperando = true;
                    break;
                }
            }
        }
        else
        {
            foreach (char caracter in input)
            {
                if (operandos.Contains(caracter))
                {
                    contieneOperando = true;
                    break;
                }
            }
        }

        if (!contieneOperando)
        {
            throw new InvalidOperationException(
                "Es necesario que se realice MINIMO una operación."
            );
        }
    }

    //Función que divide la expresión matemática en tokens y la almacena en una lista
    static List<object> TokenizarExpresion(string expresion)
    {
        // Lista para almacenar los tokens resultantes
        List<object> tokens = new List<object>();
        string token = "";

        for (int i = 0; i < expresion.Length; i++)
        {
            char caracter = expresion[i];

            // Ignorar espacios en blanco
            if (char.IsWhiteSpace(caracter))
                continue;

            if (char.IsDigit(caracter) || caracter == '.')
            {
                // Acumular caracteres numéricos para formar un número
                token += caracter;
            }
            else
            {
                if (!string.IsNullOrEmpty(token))
                {
                    // Si se encontró un operador o paréntesis, 
                    // convertir el token previo (si es un número) a double y agregarlo a la lista de tokens
                    if (double.TryParse(token, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double numero))
                    {
                        tokens.Add(numero);
                    }
                    else
                    {
                        throw new FormatException("Error al convertir el token a número: " + token);
                    }
                    token = ""; // Reiniciar el token
                }

                if (caracter == '(')
                {
                    // Insertar un operador de multiplicación implícito si hay un número antes del paréntesis
                    if (tokens.Count > 0 && tokens[tokens.Count - 1] is double)
                    {
                        tokens.Add("*");
                    }
                }

                // Agregar el operador o paréntesis a la lista de tokens
                tokens.Add(caracter.ToString());
            }
        }

        if (!string.IsNullOrEmpty(token))
        {
            // Procesar cualquier número que quede al final de la expresión
            if (double.TryParse(token, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out double numero))
            {
                tokens.Add(numero);
            }
            else
            {
                throw new FormatException("Error al convertir el token a número: " + token);
            }
        }

        return tokens;
    }

    //Función que realiza las operaciones necesarias para llegar al resultado final invocando a las demás funciones
    static double RealizarOperacion(List<object> tokens)
{
    Stack<double> numeros = new Stack<double>();
    Stack<string> operadores = new Stack<string>();

    foreach (object token in tokens)
    {
        if (token is double)
        {
            // Si el token es un número, lo apilamos en la pila de números.
            numeros.Push((double)token);
        }
        else if (token is string)
        {
            string operador = (string)token;
            if (operador == "(")
            {
                // Si encontramos un paréntesis abierto, lo apilamos en la pila de operadores.
                operadores.Push(operador);
            }
            else if (operador == ")")
            {
                // Si encontramos un paréntesis cerrado, calculamos las operaciones dentro del paréntesis.
                while (operadores.Count > 0 && operadores.Peek() != "(")
                {
                    RealizarCalculo(numeros, operadores);
                }
                if (operadores.Count > 0 && operadores.Peek() == "(")
                {
                    operadores.Pop(); // Elimina el paréntesis abierto
                }
            }
            else if (EsOperadorValido(operador))
            {
                // Si encontramos un operador válido, verificamos su prioridad y realizamos cálculos si es necesario.
                while (operadores.Count > 0 && PrioridadOperador(operador) <= PrioridadOperador(operadores.Peek()))
                {
                    RealizarCalculo(numeros, operadores);
                }
                operadores.Push(operador);
            }
            else
            {
                throw new ArgumentException("Operador no válido: " + operador);
            }
        }
    }

    // Realizamos cualquier cálculo restante.
    while (operadores.Count > 0)
    {
        RealizarCalculo(numeros, operadores);
    }

    // Al final, la pila de números debe contener el resultado final.
    if (numeros.Count != 1)
    {
        throw new InvalidOperationException("La expresión no es válida.");
    }

    return numeros.Pop();
}

static bool EsOperadorValido(string operador)
{
    // Verificamos si el operador es uno de los operadores válidos: +, -, *, /
    return operador == "+" || operador == "-" || operador == "*" || operador == "/";
}

static int PrioridadOperador(string operador)
{
    // Asignamos prioridades a los operadores, donde * y / tienen mayor prioridad que + y -.
    switch (operador)
    {
        case "+":
        case "-":
            return 1;
        case "*":
        case "/":
            return 2;
        default:
            return 0; // Operador no válido
    }
}

static void RealizarCalculo(Stack<double> numeros, Stack<string> operadores)
{
    // Realizamos una operación con los dos últimos números y el último operador.
    double operando2 = numeros.Pop();
    double operando1 = numeros.Pop();
    string operador = operadores.Pop();

    double resultado = 0;

    // Realizamos la operación adecuada según el operador.
    switch (operador)
    {
        case "+":
            resultado = operando1 + operando2;
            break;
        case "-":
            resultado = operando1 - operando2;
            break;
        case "*":
            resultado = operando1 * operando2;
            break;
        case "/":
            if (operando2 == 0)
            {
                throw new DivideByZeroException("No se puede dividir por cero.");
            }
            resultado = operando1 / operando2;
            break;
    }

    // Apilamos el resultado nuevamente en la pila de números.
    numeros.Push(resultado);
}


    // MAIN
    static void Main(string[] args)
    {
        // INPUT INGRESADO POR EL USUARIO
        string input = "3-4/(0+4)"; //No resuelve resultados que comienzan negativos. Resolver tema del rango en división que finaliza con 0

        try
        {
            // Verificamos si contiene caracteres válidos
            ValidarCaracteres(input);

            // Verificamos que no empiece ni termine con un operando
            ValidarOperadoresOperandos(input);

            // Verificamos que no tenga dos puntos decimales consecutivos ni que finalice con uno
            ValidarPuntosDecimales(input);

            // Verificamos que estén balanceados los paréntesis y que before && after hayan caracteres válidos
            ValidarParentesis(input);

            // Verificamos que no haya división por cero DIRECTA
            ValidarDivisionPorCero(input);

            // Verificamos si hay operadores repetidos
            ValidarOperandosRepetidos(input);

            // Verificamos que haya al menos un operando
            ValidarOperandoMinimo(input);

            Console.WriteLine("Ecuación: " + input);

            List<object> tokens = TokenizarExpresion(input);

            double resultado = RealizarOperacion(tokens);
            Console.WriteLine("Resultado: " + resultado);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch(DivideByZeroException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}