import http from 'k6/http';
import { sleep, check } from 'k6';

const API_URL = 'https://localhost:44323/api/forms';

export const options = {
    stages: [
        { duration: '30s', target: 10 }, // Ramp up to 10 VUs over 30 seconds
        { duration: '1m', target: 10 },  // Stay at 10 VUs for 1 minute
        { duration: '30s', target: 0 },  // Ramp down to 0 VUs over 30 seconds
    ],
};

export default function () {    
    const response = http.get(API_URL);
    
    check(response, {
        'status is 200': (r) => r.status === 200,
        'response time is less than 500ms': (r) => r.timings.duration < 500,
    });
    
    sleep(1);
}
