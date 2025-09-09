import { Component } from '@angular/core';
import { CommonModule } from '@angular/common'; // ✅ Import CommonModule

@Component({
  selector: 'app-product',
  standalone: true,
  imports: [CommonModule], // ✅ Add CommonModule here
  templateUrl: './product.html',
  styleUrls: ['./product.css']
})
export class ProductComponent {
  showDetails: boolean = false;

  product = {
    name: 'Smartphone',
    price: 29999,
    description: 'A high-quality smartphone with great features.'
  };

  toggleDetails() {
    this.showDetails = !this.showDetails;
  }
}
