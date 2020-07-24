//User information which we are getting from server.
export interface IUser {
  username: string;
  displayName: string;
  token: string;
  image?: string;
}

//to registering user
export interface IUserFormValues {
  email: string;
  password: string;
  displayName?: string;
  username?: string;
}
