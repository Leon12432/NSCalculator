import { Component } from '@angular/core';
import { ExpressionService, Expression } from '../../services/expression.service';

// HistoryComponent geeft een lijst weer van de laatste berekeningen.
// Er wordt gebruik gemaakt van dezelfde ExpressionService als in CalculatorComponent om te communiceren met de API.
@Component({
  selector: 'app-home-history',
  templateUrl: './history.component.html'
})
export class HistoryComponent {
  public expressions: Expression[];

  constructor(private expressionService: ExpressionService) { }

  // Bij initialisatie van het component worden de laatste gegevens van de service opgehaald.
  ngOnInit() {
    this.getExpressions();
  }

  // Er wordt gesubscribed op state van de service zodat de informatie update wanneer nodig
  getExpressions() {
    this.expressionService.getExpressions()
      .subscribe(expressions => this.expressions = expressions)
  }
}
