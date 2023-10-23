import { Component, OnInit } from '@angular/core';
import { TodoItemTagDashboardDto, TodoItemTagDto, TodoItemTagsClient } from '../web-api-client';


@Component({
  selector: 'app-tags',
  templateUrl: './tags.component.html',
  styleUrls: ['./tags.component.scss']
})
export class TagsComponent implements OnInit {
  tags: TodoItemTagDashboardDto[];

  constructor(
    private itemTagsClient: TodoItemTagsClient
  ) { }

  ngOnInit(): void {
    this.itemTagsClient.getDashboard().subscribe(
      result => {
        this.tags = result;
      },
      error => console.error(error)
    )
  }
}
