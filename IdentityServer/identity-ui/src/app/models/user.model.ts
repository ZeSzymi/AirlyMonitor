export class User {
    username: string;
    password: string;
}

export class LoginResponse {
    returnUrl: string;
}

export const encodeQueryData = (data: any) => {
    const ret = [];
    for (let d in data)
      ret.push(encodeURIComponent(d) + '=' + encodeURIComponent(data[d]));
    return ret.join('&');
 }