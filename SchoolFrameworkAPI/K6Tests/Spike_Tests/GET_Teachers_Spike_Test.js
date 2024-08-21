import http from 'k6/http';
import { check, sleep } from 'k6';


export const options = {
    stages: [
        {
            duration: '10s',
            target: 10
        },
        {
            duration: '1m',
            target: 10000
        },
        {
            duration: '30s',
            target: 0
        }
    ],
    thresholds: {
        http_req_duration: ['p(99)<30'], // 99% of requests must complete below 30ms
        http_req_failed: ['rate<0.05'],     // Less than 5% of requests can fail
    },
}

export default function () {
    http.get('https://localhost:44323/api/teachers');
    sleep(1);
}