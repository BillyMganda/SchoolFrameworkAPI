import http from 'k6/http';
import { sleep } from 'k6';


export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    vus: 1,
    duration: '10s'
};


export default () => {
    http.get('https://localhost:44323/api/forms/all');
    sleep(1);
};