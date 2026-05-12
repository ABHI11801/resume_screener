import { Component, Inject }from '@angular/core';
import { CommonModule }from '@angular/common';
import {  MAT_DIALOG_DATA,  MatDialogModule,  MatDialogRef} from '@angular/material/dialog';
import {  ReactiveFormsModule,  FormBuilder,  Validators} from '@angular/forms';
import {  MatFormFieldModule} from '@angular/material/form-field';
import {  MatInputModule} from '@angular/material/input';
import {  MatButtonModule} from '@angular/material/button';
import { ResumeService }from '../../services/resume.service';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';
@Component({
  selector:
    'app-upload-resume-dialog',

  standalone: true,

  imports: [
    CommonModule,
    ReactiveFormsModule,

    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],

  templateUrl:
    './upload-resume-dialog.component.html',

  styleUrls: [
    './upload-resume-dialog.component.scss'
  ]
})
export class UploadResumeDialogComponent {

  selectedFile: File | null = null;

  isLoading = false;

  form;

  constructor(
    private fb: FormBuilder,

    private resumeService:
      ResumeService,

    private dialogRef:
      MatDialogRef<
        UploadResumeDialogComponent
      >,

    @Inject(MAT_DIALOG_DATA)
    public data: any
  )
  {
    this.form = this.fb.group({

      candidateName: [
        '',
        Validators.required
      ],

      email: [
        '',
        Validators.required
      ]
    });
  }

  onFileSelected(event: any): void
  {
    this.selectedFile =
      event.target.files[0];
  }

  submit(): void
  {
    if (
      this.form.invalid ||
      !this.selectedFile
    )
    {
      return;
    }

    const formData =
      new FormData();

    formData.append(
      'candidateName',
      this.form.value.candidateName!
    );

    formData.append(
      'email',
      this.form.value.email!
    );

    formData.append(
      'jobId',
      this.data.jobId.toString()
    );

    formData.append(
      'resumeFile',
      this.selectedFile
    );

    this.isLoading = true;

    this.resumeService
      .uploadResume(formData)
      .subscribe({

        next: () =>
        {
          this.dialogRef.close(true);
        },

        error: (error) =>
        {
          console.error(error);

          this.isLoading = false;
        }
      });
  }
}