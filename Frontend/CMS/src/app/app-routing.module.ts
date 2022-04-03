import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AppComponent } from './app.component';
import { ContentComponent } from './components/content/content.component';
import { FolderComponent } from './components/folder/folder.component';
import { LoginComponent } from './components/login/login.component';
import { RegisterComponent } from './components/register/register.component';

const routes: Routes = [ 
  // { path: '', component: AppComponent },


  { path: 'login', component: LoginComponent },
  { path: 'registration', component: RegisterComponent },
  { path: 'folder/:path', component: FolderComponent },
  { path: 'home', component: ContentComponent},
  { path: '', component: LoginComponent},
  

  // { path: '**', component: AppComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
