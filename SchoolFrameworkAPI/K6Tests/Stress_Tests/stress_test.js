import http from 'k6/http';
import { check, sleep } from 'k6';

export const options = {
    stages: [
        { duration: '30s', target: 20 },  // Ramp-up to 20 users
        { duration: '1m', target: 20 },   // Stay at 20 users for 1 minute
        { duration: '30s', target: 0 },   // Ramp-down to 0 users
    ],
    thresholds: {
        http_req_duration: ['p(95)<500'], // 95% of requests should be below 500ms
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