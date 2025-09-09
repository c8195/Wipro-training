import { Component } from '@angular/core';
import { UserAuthComponent } from '../user-auth/user-auth.component';
import { ProductComponent } from '../product/product.component';
import { ShoppingCartComponent } from '../shopping-cart/shopping-cart.component';

@Component({
  selector: 'app-main-container',
  standalone: true,
  imports: [UserAuthComponent, ProductComponent, ShoppingCartComponent],
  template: `
    <div class="app">
      <h1>🛒 Angular State Management Demo</h1>
      
      <div class="components">
        <app-user-auth></app-user-auth>
        <app-product></app-product>
        <app-shopping-cart></app-shopping-cart>
      </div>

      <div class="explanation">
        <h3>📚 What we learned:</h3>
        <ul>
          <li>✅ RxJS Observables and Subjects</li>
          <li>✅ BehaviorSubject for state management</li>
          <li>✅ Shared state between components</li>
          <li>✅ Reactive programming concepts</li>
          <li>✅ NgRx Store setup (ready for next steps)</li>
        </ul>
      </div>
    </div>
  `,
  styles: [`
    .app {
      padding: 20px;
      font-family: Arial, sans-serif;
    }

    .components {
      display: flex;
      flex-wrap: wrap;
      gap: 20px;
      margin-bottom: 30px;
    }

    .explanation {
      background-color: #f0f0f0;
      padding: 20px;
      border-radius: 10px;
    }

    h1 {
      color: #1976d2;
      text-align: center;
    }

    h3 {
      color: #333;
    }
  `]
})
export class MainContainerComponent {}