import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, Observable, of } from 'rxjs';
import { map } from 'rxjs/operators';

export interface Room {
    id: string;
    name: string;
    type: string;
    price: number;
    isAvailable: boolean;
}

export interface Reservation {
    id: string;
    roomId: string;
    roomName: string;
    guestName: string;
    checkInDate: Date;
    checkOutDate: Date;
    status: 'confirmed' | 'canceled';
}

export interface RoomType {
    id: number;
    name: string;
    description: string;
    pricePerNight: number;
}

export interface Guest {
    id?: number;
    firstName: string;
    lastName: string;
    email: string;
    phoneNumber: string;
}

@Injectable({
    providedIn: 'root',
})
export class HotelService {
    private rooms: Room[] = [
        { id: '1', name: '101', type: 'Single', price: 100, isAvailable: true },
        { id: '2', name: '102', type: 'Double', price: 150, isAvailable: false },
        { id: '3', name: '201', type: 'Suite', price: 300, isAvailable: true },
    ];

    private roomsSubject = new BehaviorSubject<Room[]>(this.rooms);

    constructor(private http: HttpClient) { }

    getAvailableRooms(checkIn: string, checkOut: string): Observable<Room[]> {
        return this.http.get<any[]>(`https://localhost:7193/api/room/available?checkIn=${checkIn}&checkOut=${checkOut}`).pipe(
            map(rooms => rooms.map(r => ({
                id: r.id.toString(),
                name: r.number,
                type: r.type?.name || 'Unknown',
                price: r.type?.pricePerNight || 0,
                isAvailable: true
            })))
        );
    }

    getAllRooms(): Observable<Room[]> {
        return this.http.get<any[]>('https://localhost:7193/api/room').pipe(
            map(rooms => rooms.map(r => ({
                id: r.id.toString(),
                name: r.number,
                type: r.type?.name || 'Unknown',
                price: r.type?.pricePerNight || 0,
                isAvailable: true
            })))
        );
    }

    createRoom(room: { number: string; roomTypeId: number }): Observable<Room> {
        return this.http.post<any>('https://localhost:7193/api/room', room).pipe(
            map(r => ({
                id: r.id.toString(),
                name: r.number,
                type: r.type?.name || 'Unknown',
                price: r.type?.pricePerNight || 0,
                isAvailable: true
            }))
        );
    }

    getRoomTypes(): Observable<RoomType[]> {
        return this.http.get<RoomType[]>('https://localhost:7193/api/roomtype');
    }

    createGuest(guest: Guest): Observable<Guest> {
        return this.http.post<Guest>('https://localhost:7193/api/guest', guest);
    }

    createReservation(reservation: any): Observable<any> {
        return this.http.post<any>('https://localhost:7193/api/reservation', reservation);
    }

    removeRoom(roomId: string): Observable<void> {
        return this.http.delete<void>(`https://localhost:7193/api/room/${roomId}`);
    }

    getReservations(): Observable<Reservation[]> {
        return this.http.get<any[]>('https://localhost:7193/api/reservation').pipe(
            map(reservations => reservations.map(r => ({
                id: r.id.toString(),
                roomId: r.roomId.toString(),
                roomName: r.room?.number || 'Unknown',
                guestName: r.guest ? `${r.guest.firstName} ${r.guest.lastName}` : 'Unknown',
                checkInDate: new Date(r.checkInDate),
                checkOutDate: new Date(r.checkOutDate),
                status: 'confirmed'
            })))
        );
    }

    private isLoggedIn = false;

    cancelReservation(reservationId: string): Observable<void> {
        return this.http.delete<void>(`https://localhost:7193/api/reservation/${reservationId}`);
    }

    login(login: string, password: string): Observable<any> {
        return this.http.post<any>('https://localhost:7193/api/employee/login', { login, password }).pipe(
            map(response => {
                this.isLoggedIn = true;
                return response;
            })
        );
    }

    isAuthenticated(): boolean {
        return this.isLoggedIn;
    }
}
