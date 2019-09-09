using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSCalculator.Models;
using NSCalculator.Utility;

namespace NSCalculator.Controllers
{
    // Controller voor de 2 acties die de back-end uitvoert:
    // een expressie uitrekenen en de lijst van de laatste 5 expressies opvragen
    // Het opslaan maakt nu gebruik van Entity Framework. Hierdoor is het mogelijk een echte database te gebruiken.
    [Route("api/[controller]")]
    [ApiController]
    public class ExpressionsController : ControllerBase
    {
        private readonly NSCalculatorContext _context;

        public ExpressionsController(NSCalculatorContext context)
        {
            _context = context;
        }

        // Verkrijg de laatste 5 expressions
        // GET: api/Expressions
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Expression>>> GetExpressions()
        {
            var items = await _context.Expression.ToListAsync();
            items.Reverse();
            return Ok(items.Take(5));
        }

        // Reken een expressie uit en sla het resultaat op in de context. 
        // Kan in principe ook een POST zijn, maar omdat het opslaan van het resultaat niet de belangrijkste functie is heb ik het zo gedaan
        // GET: api/Expressions/Evaluate
        [HttpGet("Evaluate")]
        public async Task<ActionResult<Expression>> EvaluateExpression(string expression)
        {
            // Een lege expressie klopt sowieso niet
            if(expression == null || expression == "")
            {
                return BadRequest();
            }

            // Doordat het een GET-request betreft moeten tekens zoals de + escaped en unescaped worden.
            string decodedExpression = Uri.UnescapeDataString(expression);

            double result;

            // Reken de expressie uit. Een FormatException geeft aan wanneer de input ongeldig is. 
            // Door het af te vangen wordt dit ook goed aan de client doorgegeven middels een 400 response
            // De evaluatie in de controller afhandelen zou deze klasse behoorlijk vervuilen en niet herbruikbaar zijn. 
            // Vandaar een aparte klasse daarvoor.
            try
            {
                result = ExpressionEvaluator.EvaluateExpression(decodedExpression);
            }
            catch (FormatException e)
            {
                return BadRequest();
            }

            var resultingExpression = new Expression()
            {
                ExpressionText = decodedExpression,
                ExpressionResult = result.ToString(),
            };

            // Sla de expressie op zodat het in de historie terecht komt
            _context.Expression.Add(resultingExpression);
            await _context.SaveChangesAsync();

            return resultingExpression;
        }
    }
}
