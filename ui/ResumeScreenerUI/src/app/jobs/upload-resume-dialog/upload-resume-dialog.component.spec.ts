import { ComponentFixture, TestBed } from '@angular/core/testing';

import { UploadResumeDialogComponent } from './upload-resume-dialog.component';

describe('UploadResumeDialogComponent', () => {
  let component: UploadResumeDialogComponent;
  let fixture: ComponentFixture<UploadResumeDialogComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [UploadResumeDialogComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(UploadResumeDialogComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
