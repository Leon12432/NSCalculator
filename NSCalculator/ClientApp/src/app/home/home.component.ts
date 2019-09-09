import { Component } from '@angular/core';

// HomeComponent is de basis van de front-end. Deze pagina bevat de rekenmachine en een lijst van laatste berekeningen.
// Deze staan in aparte components zodat homeComponent niet teveel verantwoordelijkheden, en dus code, krijgt.
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent {
}
