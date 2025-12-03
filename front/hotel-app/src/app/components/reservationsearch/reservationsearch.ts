import { Component, Input } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HotelService, Room, Guest } from '../../services/hotel.service';

@Component({
  selector: 'app-reservationsearch',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './reservationsearch.html',
  styleUrl: './reservationsearch.css',
})
export class Reservationsearch {
  @Input() searchData: { checkin: string; checkout: string } | null = null;
  availableRooms: Room[] = [];
  selectedRoom: Room | null = null;

  guest: Guest = {
    firstName: '',
    lastName: '',
    email: '',
    phoneNumber: ''
  };

  constructor(private hotelService: HotelService) { }

  ngOnChanges() {
    if (this.searchData) {
      this.hotelService.getAvailableRooms(this.searchData.checkin, this.searchData.checkout).subscribe(rooms => {
        this.availableRooms = rooms;
      });
    }
  }

  reserveRoom(room: Room) {
    this.selectedRoom = room;
  }

  cancelReservation() {
    this.selectedRoom = null;
    this.guest = { firstName: '', lastName: '', email: '', phoneNumber: '' };
  }

  confirmReservation() {
    if (!this.selectedRoom || !this.searchData) return;

    this.hotelService.createGuest(this.guest).subscribe({
      next: (createdGuest) => {
        const reservation = {
          guestId: createdGuest.id,
          roomId: this.selectedRoom!.id,
          checkInDate: this.searchData!.checkin,
          checkOutDate: this.searchData!.checkout,
          totalPrice: this.selectedRoom!.price // Simplified calculation
        };

        this.hotelService.createReservation(reservation).subscribe({
          next: () => {
            alert('Reservation confirmed!');
            this.cancelReservation();
            // Refresh available rooms
            this.ngOnChanges();
          },
          error: (err) => {
            console.error('Error creating reservation:', err);
            alert('Failed to create reservation.');
          }
        });
      },
      error: (err) => {
        console.error('Error creating guest:', err);
        alert('Failed to create guest profile.');
      }
    });
  }
}
