import { bootstrapApplication } from '@angular/platform-browser';
import { provideRouter } from '@angular/router';
import { HTTP_INTERCEPTORS, provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

import { AppComponent } from './app/app.component';
import { routes } from './app/app.routes';
import { AuthorizationInterceptor } from './app/Interceptor/authorization.interceptor';

bootstrapApplication(AppComponent, {
  providers: [
  provideRouter(routes),
    provideHttpClient(withInterceptorsFromDi()),  
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthorizationInterceptor,
      multi: true
    }  ]
}).catch(err => console.error(err));
