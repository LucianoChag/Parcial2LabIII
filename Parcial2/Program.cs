using System.Globalization;

class Ecuacion
{
    // Función para validar que la ecuación no comience ni termine con algún operador
    static void ValidarOperadoresOperandos(string input)
    {
        List<char> operandos = new List<char>{
            '+', '-', '/', '*', '.'
        };

        char primerCaracter = input[0];
        char ultimoCaracter = input[input.Length - 1];

        if (operandos.Contains(primerCaracter) || operandos.Contains(ultimoCaracter))
        {
            if (primerCaracter != '-')
            {
                throw new InvalidOperationException("El primer y último carácter de la ecuación no pueden ser operadores o puntos decimales.");
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
                    throw new InvalidOperationException("Dos puntos decimales consecutivos no están permitidos.");
                }
                puntoDecimalEnCurso = true;
            }
            else if (!Char.IsDigit(caracterActual))
            {
                puntoDecimalEnCurso = false;
            }

            if (i == input.Length - 1 && caracterActual == '.')
            {
                throw new InvalidOperationException("El número no puede terminar con un punto decimal.");
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
                    throw new InvalidOperationException("Operador no válido después de abrir un paréntesis.");
                }
                if (caracAnterior == '.')
                {
                    throw new InvalidOperationException("Previo a un paréntesis no puede haber un '.'.");
                }
            }
            else if (caracterActual == ')')
            {
                contadorParentesis--;
                if (contadorParentesis < 0)
                {
                    throw new InvalidOperationException("Los paréntesis no están balanceados.");
                }
                if (caracAnterior == '+' || caracAnterior == '-' || caracAnterior == '*' || caracAnterior == '/' || caracAnterior == '.')
                {
                    throw new InvalidOperationException("Operador no válido antes de cerrar un paréntesis.");
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

            if (caracterActual == '/' && caracSig == '0')
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
        List<char> caracteresValidos = new List<char>{
            '+', '-', '/', '*', '(', ')', '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', '.'
        };

        foreach (char caracter in input)
        {
            if (!caracteresValidos.Contains(caracter))
            {
                char auxChar = char.ToLower(caracter);
                switch (auxChar)
                {
                    case ',':
                        throw new InvalidOperationException("Utilizó un carácter inválido: '" + caracter + "'. Prefiera el uso de '.' en lugar de este carácter.");
                    case ' ':
                        throw new InvalidOperationException("Utilizó un carácter inválido: ' '. Escriba la ecuación sin espacios.");
                    case 'x':
                        throw new InvalidOperationException("Utilizó un carácter inválido: '" + caracter + "'. Prefiera el uso de '*' en lugar de este carácter.");
                    default:
                        throw new InvalidOperationException("Utilizó un carácter inválido: " + caracter);
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
        throw new InvalidOperationException("Es necesario que se realice MINIMO una operación.");
    }
}

static List<object> TokenizarExpresion(string expresion)
//Tokeniza una expresión matemática en una lista de objetos que representan los elementos de la expresión.
// Los números se convierten en valores double, y los operadores y otros caracteres se mantienen como cadenas.
    {
        List<object> tokens = new List<object>();
        string token = "";

        for (int i = 0; i < expresion.Length; i++)
        {
            char caracter = expresion[i];

            if (char.IsWhiteSpace(caracter))
                continue;

            if (char.IsDigit(caracter) || caracter == '.')
            {
                token += caracter;
            }
            else
            {
                if (!string.IsNullOrEmpty(token))
                {
                    if (TryParseDouble(token, out double numero))
                    {
                        tokens.Add(numero);
                    }
                    else
                    {
                        throw new FormatException("Error al convertir el token a número: " + token);
                    }
                    token = "";
                }
                tokens.Add(caracter.ToString());
            }
        }

        if (!string.IsNullOrEmpty(token))
        {
            if (TryParseDouble(token, out double numero))
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

    static bool TryParseDouble(string s, out double result)
    {
        return double.TryParse(s, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out result);
    }


    // MAIN
    static void Main(string[] args)
    {
        // INPUT INGRESADO POR EL USUARIO
        string input = "2-33(4.5+9)/2";

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

        List<object> tokens = TokenizarExpresion(input);

        foreach (var token in tokens)
        {
            Console.WriteLine(token);
        }

            Console.WriteLine(input);
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}








//Entrada de datos - Proceso - Salida
