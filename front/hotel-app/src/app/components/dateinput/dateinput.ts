import { Component, Output, EventEmitter } from '@angular/core';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-dateinput',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './dateinput.html',
  styleUrl: './dateinput.css',
})
export class Dateinput {
  checkinDate: string = '';
  checkoutDate: string = '';

  @Output() search = new EventEmitter<{ checkin: string; checkout: string }>();

  onSearch() {
    if (this.checkinDate && this.checkoutDate) {
      this.search.emit({
        checkin: this.checkinDate,
        checkout: this.checkoutDate,
      });
    }
  }
}
