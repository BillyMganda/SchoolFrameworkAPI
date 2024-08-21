import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '15m', target: 100 },   // Ramp-up to 100 users over 15 minutes
        { duration: '4h', target: 100 },    // Stay at 100 users for 4 hours
        { duration: '15m', target: 0 },     // Ramp-down to 0 users over 15 minutes
    ],
    thresholds: {
        http_req_duration: ['p(95)<600'],   // 95% of requests should be below 600ms
        http_req_failed: ['rate<0.01'],     // Less than 1% of requests can fail
        'checks{status is 201}': ['rate>0.99'], // 99% of requests should return 201
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
        'response time OK': (r) => r.timings.duration < 600,
    });

    sleep(Math.random() * 5 + 2); // Random sleep between 2 and 7 seconds
}