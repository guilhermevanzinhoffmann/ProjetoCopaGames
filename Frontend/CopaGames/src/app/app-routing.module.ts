import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { ResultComponent } from './result/result.component';
import { SelectionComponent } from './selection/selection.component';

const routes: Routes = [
  {path:'', redirectTo: '/selection', pathMatch: 'full'},
  {path:'selection', component: SelectionComponent},
  {path:'result', component: ResultComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
