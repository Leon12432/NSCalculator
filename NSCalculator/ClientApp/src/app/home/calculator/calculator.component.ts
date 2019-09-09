import { Component } from '@angular/core';
import { ExpressionService } from '../../services/expression.service';

// CalculatorComponent is verantwoordelijk voor het weergeven van de calculator en het afhandelen van de invoer hiervan.
// Deze gebruikt de expressionService om expressies uit te voeren via de API.
@Component({
  selector: 'app-home-calculator',
  templateUrl: './calculator.component.html',
})
export class CalculatorComponent {
  private currentExpression = "";
  public showBadRequestPopup = false;

  constructor(private expressionService: ExpressionService) {
  }

  // Voeg een teken toe aan de invoer
  public addInput(input: string) {
    this.currentExpression += input;
  }

  // Verwijder het laatste teken van de invoer
  public deleteInput() {
    if (this.currentExpression.length < 1) return;
    this.currentExpression = this.currentExpression.slice(0, -1);
  }

  // Evalueer de invoer door het te versturen naar de server en het resultaat te ontvangen.
  public evaluateInput() {
    this.expressionService.evaluateExpression(this.currentExpression)
      .subscribe(result => {
        this.currentExpression = result.expressionResult;
        this.expressionService.getExpressions();
        this.showBadRequestPopup = false;
      }, error => this.showBadRequestPopup = true);
  }
}
