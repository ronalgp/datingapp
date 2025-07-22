import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { RegisterCreds, User } from '../../../types/user';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private accoutService = inject(AccountService);
  cancelRegister = output<boolean>();
  protected creds = {} as RegisterCreds;

  register() {
    this.accoutService.register(this.creds).subscribe({
      next: response => {
        console.log('Registration successful', response);
        this.cancelRegister.emit(false);
      },
      error: (error) => {
        console.error('Registration failed', error);
      },
    });
  }

  cancel() {
    this.cancelRegister.emit(false);
  }
}
