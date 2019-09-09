import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable } from 'rxjs';

// ExpressionService wordt gebruikt voor communicatie met de API. Meerdere components maken hiervan gebruik.
// Op deze manier hoeft alleen de service te worden aangepast bij wijzigingen aan de API.
@Injectable({
  providedIn: 'root'
})
export class ExpressionService {
  private expressions: BehaviorSubject<Expression[]> = new BehaviorSubject<Expression[]>([]);

  constructor(private httpClient: HttpClient, @Inject('BASE_URL') private baseUrl: string) {
  }

  // Voer een API call uit om een berekening uit te voeren
  public evaluateExpression(expression: string): Observable<Expression> {
    let params = new HttpParams();
    // Encoding is nodig om tekens zoals de + mee te kunnen sturen
    params = params.append('expression', encodeURIComponent(expression));

    let result = this.httpClient.get<Expression>(this.baseUrl + 'api/Expressions/Evaluate/', { params });
    return result;
  }

  // Voer een API call uit om de laatste 5 berekeningen op te halen. Voer .next() uit om alle subscribers te informeren over de nieuwe state
  public getExpressions(): Observable<Expression[]> {
    this.httpClient.get<Expression[]>(this.baseUrl + 'api/Expressions')
      .subscribe(result => this.expressions.next(result));
    return this.expressions.asObservable();
  }
}

export interface Expression {
  expressionText?: string;
  expressionResult?: string;
}
