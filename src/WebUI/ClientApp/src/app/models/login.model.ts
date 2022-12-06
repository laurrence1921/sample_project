export class LoginModel {
  email: string;
  password: string;
  loginType: LoginType;
  oAuthToken: string;
  oAuthId: string;
  rememberMe: boolean;
  photoUrl: string;
  name: string;
  socialToken: string;
}

export enum LoginType {
  Email = 1,
  Google = 2
}
