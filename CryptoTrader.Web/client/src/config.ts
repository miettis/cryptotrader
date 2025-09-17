export function getApiUrl() {
  if (process.env.DEV) {
    return 'https://localhost:32776';
  }
  return location.protocol + '//' + location.host;
}
