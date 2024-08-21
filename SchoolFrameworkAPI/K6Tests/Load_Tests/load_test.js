import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '5m', target: 50 },    // Ramp-up to 50 users over 5 minutes
        { duration: '30m', target: 50 },   // Stay at 50 users for 30 minutes
        { duration: '5m', target: 0 },     // Ramp-down to 0 users over 5 minutes
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'],  // 95% of requests should be below 500ms
        http_req_failed: ['rate<0.01'],    // Less than 1% of requests can fail
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

    sleep(Math.random() * 3 + 1); // Random sleep between 1 and 4 seconds
}