import { Component, EventEmitter, Input, Output } from '@angular/core';
import { FileService } from '../../../core/services/file.service';
import { NotificationService } from '../../../core/services/notification.service';

@Component({
  selector: 'app-image-upload',
  templateUrl: './image-upload.component.html',
  styleUrls: ['./image-upload.component.scss']
})
export class ImageUploadComponent {
  @Input() multiple: boolean = true;
  @Input() maxFiles: number = 5;
  @Output() filesSelected = new EventEmitter<File[]>();
  @Output() filesRemoved = new EventEmitter<number>();

  selectedFiles: File[] = [];
  dragOver: boolean = false;

  constructor(
    private fileService: FileService,
    private notificationService: NotificationService
  ) {}

  onFileSelect(event: any): void {
    const files = Array.from(event.target.files) as File[];
    this.processFiles(files);
  }

  onDragOver(event: DragEvent): void {
    event.preventDefault();
    this.dragOver = true;
  }

  onDragLeave(event: DragEvent): void {
    event.preventDefault();
    this.dragOver = false;
  }

  onDrop(event: DragEvent): void {
    event.preventDefault();
    this.dragOver = false;
    
    const files = Array.from(event.dataTransfer?.files || []) as File[];
    this.processFiles(files);
  }

  private processFiles(files: File[]): void {
    const validFiles: File[] = [];
    
    for (const file of files) {
      const validation = this.fileService.validateImageFile(file);
      if (validation.valid) {
        validFiles.push(file);
      } else {
        this.notificationService.showError(validation.error || 'Invalid file');
      }
    }

    if (validFiles.length > 0) {
      if (this.multiple) {
        const totalFiles = this.selectedFiles.length + validFiles.length;
        if (totalFiles > this.maxFiles) {
          this.notificationService.showWarning(`Maximum ${this.maxFiles} files allowed`);
          return;
        }
        this.selectedFiles.push(...validFiles);
      } else {
        this.selectedFiles = [validFiles [0]];
      }
      
      this.filesSelected.emit(this.selectedFiles);
    }
  }

  removeFile(index: number): void {
    this.selectedFiles.splice(index, 1);
    this.filesRemoved.emit(index);
    this.filesSelected.emit(this.selectedFiles);
  }

  clearFiles(): void {
    this.selectedFiles = [];
    this.filesSelected.emit(this.selectedFiles);
  }

  getFileSize(bytes: number): string {
    if (bytes === 0) return '0 Bytes';
    const k = 1024;
    const sizes = ['Bytes', 'KB', 'MB', 'GB'];
    const i = Math.floor(Math.log(bytes) / Math.log(k));
    return parseFloat((bytes / Math.pow(k, i)).toFixed(2)) + ' ' + sizes[i];
  }
}