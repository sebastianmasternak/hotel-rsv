import { Component } from '@angular/core';
import { Header } from '../header/header';
import { Dateinput } from '../dateinput/dateinput';
import { Reservationsearch } from '../reservationsearch/reservationsearch';

@Component({
    selector: 'app-home',
    standalone: true,
    imports: [Header, Dateinput, Reservationsearch],
    templateUrl: './home.html',
    styleUrl: './home.css',
})
export class Home {
    searchData: { checkin: string; checkout: string } | null = null;

    handleSearch(data: { checkin: string; checkout: string }) {
        this.searchData = data;
    }
}
