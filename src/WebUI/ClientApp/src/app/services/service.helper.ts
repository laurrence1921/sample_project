import { environment } from '../../environments/environment';

export function getAPI(): string {
  return environment.serverUrl;
}

