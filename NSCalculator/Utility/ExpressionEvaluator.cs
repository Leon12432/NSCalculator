using System;
using System.Linq;

namespace NSCalculator.Utility
{
    // Eenvoudige expressie parser/uitrekenaar. 
    // Werkt door steeds recursief een expressie af te gaan tot er operators van het huidige precedentieniveau gevonden worden.
    // De berekening van deze operator wordt dan uitgevoerd. Wanneer alle operators van het huidige precedentieniveau klaar zijn,
    // wordt het volgende lagere niveau uitgevoerd. Huidige mogelijkheden: Operators *, /, +, -, decimale getallen en negatieve getallen
    // Er kunnen eenvoudig nieuwe operators worden toegevoegd zonder grote wijzigingen door het gebruik van een operator-array met de bijbehorende implementaties.
    // Uiteraard zijn er ook wel externe libraries(zoals NCalc), omwegen binnen het .NET framework(zoals DataTable.Compute())
    // of al eerder geschreven algoritmes op StackOverflow, maar zelf iets proberen is natuurlijk ook leuk
    public class ExpressionEvaluator
    {
        static readonly Operator[] operators = new Operator[] {
            new Operator() {Token = '^', Operation = (double a, double b) => Math.Pow(a, b), Precedence = 2 },
            new Operator() {Token = '*', Operation = (double a, double b) => a * b, Precedence = 1 },
            new Operator() {Token = '/', Operation = (double a, double b) => a / b, Precedence = 1 },
            new Operator() {Token = '+', Operation = (double a, double b) => a + b, Precedence = 0 },
            new Operator() {Token = '-', Operation = (double a, double b) => a - b, Precedence = 0 },
        };

        // Publieke functie voor evaluaties afhandelen
        public static double EvaluateExpression(string exp)
        {
            for (int i = operators.Max(op => op.Precedence); i >= 0; i--)
            {
                exp = Evaluate(exp, i);
            }
            return double.Parse(exp);
        }

        // Recursieve evaluatiemethode
        static private string Evaluate(string exp, int currentPrecedence)
        {
            // Steeds de eerste operator met eerste operand en volgende expressie ophalen
            var parsed = ParseNextExpression(exp);

            // Geeft aan of er alleen een 1e operand is gevonden zonder operatie,
            // hier hoeft niks mee te worden gedaan
            if (!parsed.Success)
            {
                return parsed.Operand1.ToString();
            }

            // Moet de gevonden operator in deze aanroep worden uitgevoerd?
            if (parsed.Operator.Precedence == currentPrecedence)
            {
                //Volgende operatie bepalen om 2e operand te verkrijgen
                var nextExpression = ParseNextExpression(parsed.Operand2);
                var evaluated = parsed.Operator.Operation(parsed.Operand1, nextExpression.Operand1).ToString();

                // Return met of zonder volgende expressie
                if(nextExpression.Operator != null)
                {
                    return Evaluate(evaluated + nextExpression.Operator.Token + nextExpression.Operand2, currentPrecedence);
                }
                else
                {
                    return Evaluate(evaluated, currentPrecedence);
                }
            }
            // Operatie laten staan voor volgende aanroepen
            else
            {
                var evaluated = Evaluate(parsed.Operand2, currentPrecedence);
                return parsed.Operand1 + parsed.Operator.Token.ToString() + evaluated;
            }
        }

        // Verdeel een expressie in 1e operand, operator en de rest van de expressie
        // Voorbeeld: 2*3*2 wordt {2, '*', "3*2"}
        static private ParsedExpression ParseNextExpression(string exp)
        {
            var parsed = new ParsedExpression();
            for (int i = 0; i < exp.Length; i++)
            {
                if (operators.Any(op => op.Token == exp[i]))
                {
                    // Het minteken is de enige operator die aan het begin van een operand mag staan
                    if (i == 0 && exp[i] == '-')
                    {
                        continue;
                    }
                    parsed.Operand1 = double.Parse(exp.Substring(0, i));
                    parsed.Operator = operators.Where(op => op.Token == exp[i]).First();
                    parsed.Operand2 = exp.Substring(i + 1, exp.Length - i - 1);
                    parsed.Success = true;
                    return parsed;
                }
            }
            parsed.Operand1 = double.Parse(exp);
            parsed.Success = false;
            return parsed;
        }
    }

    class Operator
    {
        public char Token { get; set; }
        public Func<double, double, double> Operation { get; set; }
        public int Precedence { get; set; }
    }

    class ParsedExpression
    {
        public double Operand1 { get; set; }
        public string Operand2 { get; set; }
        public Operator Operator { get; set; }
        public bool Success { get; set; }
    }
}
