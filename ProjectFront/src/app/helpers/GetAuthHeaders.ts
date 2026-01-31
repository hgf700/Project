import { HttpHeaders } from '@angular/common/http';

export function getAuthHeaders(): HttpHeaders {
  const token = localStorage.getItem('jwt') || '';
  return new HttpHeaders({
    Authorization: `Bearer ${token}`
  });
}