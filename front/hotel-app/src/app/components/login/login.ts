import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { Header } from '../header/header';
import { HotelService } from '../../services/hotel.service';

@Component({
    selector: 'app-login',
    standalone: true,
    imports: [Header, FormsModule],
    templateUrl: './login.html',
    styleUrl: './login.css',
})
export class Login {
    username = '';
    password = '';

    constructor(private router: Router, private hotelService: HotelService) { }

    onLogin() {
        this.hotelService.login(this.username, this.password).subscribe({
            next: (response) => {
                console.log('Login successful', response);
                this.router.navigate(['/admin']);
            },
            error: (err) => {
                console.error('Login failed', err);
                alert('Invalid credentials');
            }
        });
    }
}
