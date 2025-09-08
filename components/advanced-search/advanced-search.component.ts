import { Component, Output, EventEmitter, OnInit, OnDestroy } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { Subject, takeUntil, debounceTime, distinctUntilChanged } from 'rxjs';
import { EnhancedQuestionService, QuestionFilters } from '../../../core/services/enhanced-question.service';

@Component({
  selector: 'app-advanced-search',
  templateUrl: './advanced-search.component.html',
  styleUrls: ['./advanced-search.component.css']
})
export class AdvancedSearchComponent implements OnInit, OnDestroy {
  @Output() filtersChanged = new EventEmitter<QuestionFilters>();
  @Output() searchTriggered = new EventEmitter<void>();

  searchForm: FormGroup;
  isExpanded = false;
  popularTags: string[] = [];
  suggestions: string[] = [];
  showSuggestions = false;
  isLoading = false;

  private destroy$ = new Subject<void>();

  sortOptions = [
    { value: 'newest', label: 'Newest First', icon: 'fas fa-clock' },
    { value: 'oldest', label: 'Oldest First', icon: 'fas fa-history' },
    { value: 'votes', label: 'Most Voted', icon: 'fas fa-thumbs-up' },
    { value: 'activity', label: 'Recent Activity', icon: 'fas fa-bolt' },
    { value: 'views', label: 'Most Viewed', icon: 'fas fa-eye' }
  ];

  statusOptions = [
    { value: 'all', label: 'All Questions', icon: 'fas fa-list' },
    { value: 'approved', label: 'Approved Only', icon: 'fas fa-check' },
    { value: 'pending', label: 'Pending Review', icon: 'fas fa-hourglass-half' },
    { value: 'rejected', label: 'Rejected', icon: 'fas fa-times' }
  ];

  dateRangeOptions = [
    { value: 'all', label: 'All Time', icon: 'fas fa-infinity' },
    { value: 'today', label: 'Today', icon: 'fas fa-calendar-day' },
    { value: 'week', label: 'This Week', icon: 'fas fa-calendar-week' },
    { value: 'month', label: 'This Month', icon: 'fas fa-calendar-alt' },
    { value: 'year', label: 'This Year', icon: 'fas fa-calendar' }
  ];

  constructor(
    private formBuilder: FormBuilder,
    private questionService: EnhancedQuestionService
  ) {
    this.searchForm = this.formBuilder.group({
      search: [''],
      topic: [''],
      tags: [[]],
      sortBy: ['newest'],
      status: ['approved'],
      dateRange: ['all']
    });
  }

  ngOnInit(): void {
    this.loadPopularTags();
    this.setupSearchSuggestions();
    this.setupFormValueChanges();
  }

  ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  private setupFormValueChanges(): void {
    this.searchForm.valueChanges
      .pipe(
        takeUntil(this.destroy$),
        debounceTime(300),
        distinctUntilChanged()
      )
      .subscribe(value => {
        this.filtersChanged.emit(this.getFilters());
      });
  }

  private setupSearchSuggestions(): void {
    const searchControl = this.searchForm.get('search');
    if (searchControl) {
      searchControl.valueChanges
        .pipe(
          takeUntil(this.destroy$),
          debounceTime(300),
          distinctUntilChanged()
        )
        .subscribe(query => {
          if (query && query.length >= 2) {
            this.loadSuggestions(query);
          } else {
            this.hideSuggestions();
          }
        });
    }
  }

  private loadPopularTags(): void {
    this.questionService.getPopularTags()
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (tags) => {
          this.popularTags = tags;
        },
        error: (error) => {
          console.error('Error loading popular tags:', error);
        }
      });
  }

  private loadSuggestions(query: string): void {
    this.isLoading = true;
    this.questionService.searchSuggestions(query)
      .pipe(takeUntil(this.destroy$))
      .subscribe({
        next: (suggestions) => {
          this.suggestions = suggestions;
          this.showSuggestions = suggestions.length > 0;
          this.isLoading = false;
        },
        error: (error) => {
          console.error('Error loading suggestions:', error);
          this.isLoading = false;
        }
      });
  }

  toggleExpanded(): void {
    this.isExpanded = !this.isExpanded;
  }

  selectSuggestion(suggestion: string): void {
    this.searchForm.patchValue({ search: suggestion });
    this.hideSuggestions();
    this.onSearch();
  }

  hideSuggestions(): void {
    this.showSuggestions = false;
    this.suggestions = [];
  }

  addTag(tag: string): void {
    const currentTags = this.searchForm.get('tags')?.value || [];
    if (!currentTags.includes(tag)) {
      this.searchForm.patchValue({ tags: [...currentTags, tag] });
    }
  }

  removeTag(tag: string): void {
    const currentTags = this.searchForm.get('tags')?.value || [];
    this.searchForm.patchValue({ tags: currentTags.filter((t: string) => t !== tag) });
  }

  clearFilters(): void {
    this.searchForm.reset({
      search: '',
      topic: '',
      tags: [],
      sortBy: 'newest',
      status: 'approved',
      dateRange: 'all'
    });
  }

  onSearch(): void {
    this.hideSuggestions();
    this.searchTriggered.emit();
  }

  getFilters(): QuestionFilters {
    const formValue = this.searchForm.value;
    return {
      search: formValue.search || undefined,
      topic: formValue.topic || undefined,
      tags: formValue.tags?.length ? formValue.tags : undefined,
      sortBy: formValue.sortBy,
      status: formValue.status,
      dateRange: formValue.dateRange
    };
  }

  hasActiveFilters(): boolean {
    const filters = this.getFilters();
    return !!(
      filters.search ||
      filters.topic ||
      filters.tags?.length ||
      filters.status !== 'approved' ||
      filters.dateRange !== 'all' ||
      filters.sortBy !== 'newest'
    );
  }

  getActiveFilterCount(): number {
    const filters = this.getFilters();
    let count = 0;
    if (filters.search) count++;
    if (filters.topic) count++;
    if (filters.tags?.length) count += filters.tags.length;
    if (filters.status !== 'approved') count++;
    if (filters.dateRange !== 'all') count++;
    if (filters.sortBy !== 'newest') count++;
    return count;
  }
}