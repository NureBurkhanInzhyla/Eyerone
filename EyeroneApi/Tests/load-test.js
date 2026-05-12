import http from 'k6/http'; 
import { sleep } from 'k6';

export const options = {
    stages: [
        { duration: '30s', target: 30 },
        { duration: '1m', target: 30 },
        { duration: '30s', target: 0 },
    ],
};

export default () => {
    const url = 'http://localhost:8080/api/Users/login';
    const payload = JSON.stringify({
        email: 'user@example.com',
        password: '12345678',
    });

    const params = { headers: { 'Content-Type': 'application/json' } };
    http.post(url, payload, params);

    sleep(1);
}