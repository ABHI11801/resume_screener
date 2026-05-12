import { Component }from '@angular/core';
import { CommonModule }from '@angular/common';
import {  ReactiveFormsModule,  FormBuilder,  Validators,FormArray} from '@angular/forms';
import {  MatDialogModule,  MatDialogRef} from '@angular/material/dialog';
import {  MatFormFieldModule} from '@angular/material/form-field';
import {  MatInputModule} from '@angular/material/input';
import {  MatButtonModule} from '@angular/material/button';
import {  MatSelectModule} from '@angular/material/select';
import { JobService }from '../../services/job.service';
import { MatIconModule } from '@angular/material/icon';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-add-job-dialog',

  standalone: true,

  imports: [
    CommonModule,
    ReactiveFormsModule,

    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSelectModule,
    MatIconModule,
    MatProgressSpinnerModule
  ],

  templateUrl:
    './add-job-dialog.component.html',

  styleUrls: [
    './add-job-dialog.component.scss'
  ]
})
export class AddJobDialogComponent {

  isLoading = false;

  jobForm;

  constructor(
    private fb: FormBuilder,

    private jobService:
      JobService,

    private dialogRef:
      MatDialogRef<
        AddJobDialogComponent
      >
  )
  {
    this.jobForm = this.fb.group({
      

      title: [
        '',
        Validators.required
      ],

      department: [
        '',
        Validators.required
      ],

      description: [
        '',
        Validators.required
      ],

      minimumScore: [
        50,
        Validators.required
      ],

      status: [
        'Open',
        Validators.required
      ],
      skills: this.fb.array([])
    });
    this.addSkill();
  }
  get skills(): FormArray
  {
    return this.jobForm.get(
      'skills'
    ) as FormArray;
  }
  addSkill(): void
  {
    const skillForm =
      this.fb.group({

        skillName: [
          '',
          Validators.required
        ],

        weight: [
          5,
          Validators.required
        ],

        isRequired: [
          true
        ]
      });

    this.skills.push(skillForm);
  }
  removeSkill(index: number): void
  {
    this.skills.removeAt(index);
  }

  submit(): void
  {
    if (this.jobForm.invalid)
    {
      return;
    }

    this.isLoading = true;

    this.jobService
      .createJob(
        this.jobForm.value
      )
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