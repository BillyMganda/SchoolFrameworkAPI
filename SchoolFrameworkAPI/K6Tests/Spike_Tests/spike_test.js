import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '10s', target: 10 },   // Ramp-up to 10 users
        { duration: '1m', target: 10 },    // Stay at 10 users
        { duration: '10s', target: 100 },  // Spike to 100 users
        { duration: '3m', target: 100 },   // Stay at 100 users for 3 minutes
        { duration: '10s', target: 10 },   // Scale back to 10 users
        { duration: '3m', target: 10 },    // Stay at 10 users
        { duration: '10s', target: 0 },    // Scale down to 0 users
    ],
    thresholds: {
        http_req_duration: ['p(99)<1500'], // 99% of requests must complete below 1.5s
        http_req_failed: ['rate<0.1'],     // Less than 10% of requests can fail
    },
};

export default function () {
    const url = 'https://localhost:44323/api/students';
    const payload = JSON.stringify({
        firstName: 'John',
        lastName: 'Doe',
        dateOfBirth: '2000-01-01',
        parentOrGuardianFirstName: 'Jane',
        parentOrGuardianLastName: 'Doe',
        parentOrGuardianPhoneNumber: '1234567890',
        parentOrGuardianEmailAddress: 'jane.doe@example.com',
        formId: 1
    });

    const params = {
        headers: {
            'Content-Type': 'application/json',
        },
    };

    const res = http.post(url, payload, params);

    check(res, {
        'status is 201': (r) => r.status === 201,
        'response has location header': (r) => r.headers.hasOwnProperty('Location'),
    });

    sleep(1);
}