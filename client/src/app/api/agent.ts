import axios, { AxiosResponse } from 'axios';
import { IActivity } from './../models/Activity';
import { toast } from 'react-toastify';
import { IUser, IUserFormValues } from './../models/User';

axios.defaults.baseURL = 'http://localhost:5000/api';

// Handle HTTP responses.
axios.interceptors.response.use(undefined, (error) => {
  const { status, data, config } = error.response.status;
  if (error.message === 'Network Error' && !error.response) {
    toast.error('Network error - make sure the API is running!');
  }
  if (status === 404) {
    throw error.response;
    // history.push('/notfound');
  }
  if (
    status === 400 &&
    config.method === 'get' &&
    data.errors.hasOwnProperty('id')
  ) {
    // history.push('/notfound');
  }
  if (status === 500) {
    toast.error('Server error - check terimnal for more info!');
  }
  throw error.response;
});

const responseBody = (response: AxiosResponse) => response.data;

// Add delay to the API requests.
const sleep = (ms: number) => (response: AxiosResponse) =>
  new Promise<AxiosResponse>((resolve) =>
    setTimeout(() => resolve(response), ms)
  );

// Handle requests.
const requests = {
  get: (url: string) => axios.get(url).then(sleep(1000)).then(responseBody),
  post: (url: string, body: {}) => axios.post(url, body).then(responseBody),
  put: (url: string, body: {}) => axios.put(url, body).then(responseBody),
  delete: (url: string) => axios.post(url).then(responseBody),
};

const Activities = {
  list: (): Promise<IActivity[]> => requests.get('/activities'),
  details: (id: string) => requests.get(`/activities/${id}`),
  create: (activity: IActivity) => requests.post(`/activities`, activity),
  update: (activity: IActivity) =>
    requests.put(`/activities/${activity.id}`, activity),
  delete: (id: string) => requests.delete(`/activities/${id}`),
};

const User = {
  current: (): Promise<IUser> => requests.get('/user'),
  login: (user: IUserFormValues): Promise<IUser> => requests.post(`/user/login`, user),
  register: (user: IUserFormValues): Promise<IUser> => requests.post(`/user/register`, user),
}

export default {
  Activities,
  User
};
