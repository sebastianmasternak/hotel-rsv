import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HotelService, Room, Reservation, RoomType } from '../../services/hotel.service';
import { Header } from '../header/header';

@Component({
    selector: 'app-admin',
    standalone: true,
    imports: [CommonModule, FormsModule, Header],
    templateUrl: './admin.html',
    styleUrl: './admin.css',
})
export class Admin implements OnInit {
    rooms: Room[] = [];
    reservations: Reservation[] = [];
    roomTypes: RoomType[] = [];

    newRoom = {
        number: '',
        roomTypeId: 1
    };

    constructor(private hotelService: HotelService, private router: Router) { }

    ngOnInit(): void {
        if (!this.hotelService.isAuthenticated()) {
            this.router.navigate(['/login']);
            return;
        }
        this.hotelService.getRoomTypes().subscribe((types) => {
            this.roomTypes = types;
            if (this.roomTypes.length > 0) {
                this.newRoom.roomTypeId = this.roomTypes[0].id;
            }
        });
        this.hotelService.getAllRooms().subscribe((rooms) => (this.rooms = rooms));
        this.hotelService.getReservations().subscribe((reservations) => (this.reservations = reservations));
    }

    addRoom(): void {
        if (this.newRoom.number && this.newRoom.roomTypeId > 0) {
            this.hotelService.createRoom(this.newRoom).subscribe(room => {
                this.rooms.push(room);
                this.newRoom = { number: '', roomTypeId: 1 }; // Reset form
            });
        }
    }

    removeRoom(roomId: string): void {
        this.hotelService.removeRoom(roomId).subscribe({
            next: () => {
                this.rooms = this.rooms.filter(r => r.id !== roomId);
            },
            error: (err) => {
                console.error('Error deleting room:', err);
                alert('Failed to delete room. It might have active reservations.');
            }
        });
    }

    cancelReservation(reservationId: string): void {
        this.hotelService.cancelReservation(reservationId).subscribe({
            next: () => {
                this.reservations = this.reservations.filter(r => r.id !== reservationId);
            },
            error: (err) => {
                console.error('Error cancelling reservation:', err);
                alert('Failed to cancel reservation.');
            }
        });
    }
}
