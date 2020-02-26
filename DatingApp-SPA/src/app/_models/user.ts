import { Photo } from './photo';

export interface User {
    id: number;
    username: string;
    knownas: string;
    age: number;
    gender: string;
    created: Date;
    lastActive: Date;
    photoUrl: string;
    city: string;
    country: string;
    interests?: string;
    // this is an optinal or nullable type
    introduction?: string;
    // this is an optinal or nullable type
    lookingFor?: string;
    // our will be of type photo which is an array
    photos?: Photo[];
}