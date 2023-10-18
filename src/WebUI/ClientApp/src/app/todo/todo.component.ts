import { Component, TemplateRef, OnInit } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { settings } from 'cluster';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import {
  TodoListsClient, TodoItemsClient,
  TodoListDto, TodoItemDto, PriorityLevelDto,
  CreateTodoListCommand, UpdateTodoListCommand,
  CreateTodoItemCommand, UpdateTodoItemDetailCommand, TodoItemTagDto, UpdateTodoItemTagCommand, TodoItemTagsClient, CreateTodoItemTagCommand, TodoItemTag
} from '../web-api-client';
import { filter } from 'rxjs';

@Component({
  selector: 'app-todo-component',
  templateUrl: './todo.component.html',
  styleUrls: ['./todo.component.scss']
})
export class TodoComponent implements OnInit {
  debug = false;
  deleting = false;
  deletingTag = false;
  deleteCountDown = 0;
  deleteCountDownInterval: any;
  lists: TodoListDto[];
  unfilteredLists: TodoListDto[];
  tags: TodoItemTagDto[];
  priorityLevels: PriorityLevelDto[];
  selectedList: TodoListDto;
  selectedItem: TodoItemDto;
  newListEditor: any = {};
  newTagsEditor: any = {};
  listOptionsEditor: any = {};
  newListModalRef: BsModalRef;
  newTagModalRef: BsModalRef;
  listOptionsModalRef: BsModalRef;
  deleteListModalRef: BsModalRef;
  itemDetailsModalRef: BsModalRef;
  itemDetailsFormGroup = this.fb.group({
    id: [null],
    listId: [null],
    priority: [''],
    note: [''],
    colour:[null]
  });
  itemDetailsTagFormGroup = this.fb.group({
    id: [null],
    itemId: [null],
    title: [''],
    isDeleted: false,
    colour: [null],
  });

  constructor(
    private listsClient: TodoListsClient,
    private itemsClient: TodoItemsClient,
    private itemTagsClient: TodoItemTagsClient,
    private modalService: BsModalService,
    private fb: FormBuilder
  ) { }

  ngOnInit(): void {
    this.listsClient.get().subscribe(
      result => {
        this.lists = result.lists;
        this.unfilteredLists = JSON.parse(JSON.stringify(this.lists));
        this.priorityLevels = result.priorityLevels;
        if (this.lists.length) {
          this.selectedList = this.lists[0];
        }
      },
      error => console.error(error)
    );
  }

  // Lists
  remainingItems(list: TodoListDto): number {
    return list.items.filter(t => !t.done).length;
  }

  showNewListModal(template: TemplateRef<any>): void {
    this.newListModalRef = this.modalService.show(template);
    setTimeout(() => document.getElementById('title').focus(), 250);
  }

  newListCancelled(): void {
    this.newListModalRef.hide();
    this.newListEditor = {};
  }

  newTagCancelled(): void {
    this.newTagModalRef.hide();
    this.newTagsEditor = {};
  }

  addList(): void {
    const list = {
      id: 0,
      title: this.newListEditor.title,
      items: []
    } as TodoListDto;

    this.listsClient.create(list as CreateTodoListCommand).subscribe(
      result => {
        list.id = result;
        this.lists.push(list);
        this.selectedList = list;
        this.newListModalRef.hide();
        this.newListEditor = {};
      },
      error => {
        const errors = JSON.parse(error.response);

        if (errors && errors.Title) {
          this.newListEditor.error = errors.Title[0];
        }

        setTimeout(() => document.getElementById('title').focus(), 250);
      }
    );
  }

  showListOptionsModal(template: TemplateRef<any>) {
    this.listOptionsEditor = {
      id: this.selectedList.id,
      title: this.selectedList.title,
      colour: this.selectedList.colour
    };

    this.listOptionsModalRef = this.modalService.show(template);
  }

  updateListOptions() {
    const list = this.listOptionsEditor as UpdateTodoListCommand;
    this.listsClient.update(this.selectedList.id, list).subscribe(
      () => {
        (this.selectedList.title = this.listOptionsEditor.title),
          this.listOptionsModalRef.hide();
        this.selectedList.colour = this.listOptionsEditor.colour;
        this.listOptionsEditor = {};
      },
      error => console.error(error)
    );
  }

  confirmDeleteList(template: TemplateRef<any>) {
    this.listOptionsModalRef.hide();
    this.deleteListModalRef = this.modalService.show(template);
  }

  deleteListConfirmed(): void {
    this.listsClient.delete(this.selectedList.id).subscribe(
      () => {
        this.deleteListModalRef.hide();
        this.lists = this.lists.filter(t => t.id !== this.selectedList.id);
        this.selectedList = this.lists.length ? this.lists[0] : null;
      },
      error => console.error(error)
    );
  }

  // Items
  showItemDetailsModal(template: TemplateRef<any>, item: TodoItemDto): void {
    this.selectedItem = item;
    this.itemDetailsFormGroup.patchValue(this.selectedItem);

    this.itemDetailsModalRef = this.modalService.show(template);
    this.itemDetailsModalRef.onHidden.subscribe(() => {
        this.stopDeleteCountDown();
    });
  }

  showNewTagModal(template: TemplateRef<any>, item: TodoItemDto): void {
    const tag = {
      id: 0,
      itemId: item.id,
    } as TodoItemTagDto;

    this.itemDetailsTagFormGroup.patchValue(tag);
    this.newTagModalRef = this.modalService.show(template);
  }

  showEditTagModal(template: TemplateRef<any>, tag: TodoItemTagDto, item: TodoItemDto): void {
    this.selectedItem = item;
    this.itemDetailsTagFormGroup.patchValue(tag);
    this.newTagModalRef = this.modalService.show(template);
  }

  updateItemDetails(): void {
    const item = new UpdateTodoItemDetailCommand(this.itemDetailsFormGroup.value);
    this.itemsClient.updateItemDetails(this.selectedItem.id, item).subscribe(
      () => {
        if (this.selectedItem.listId !== item.listId) {
          this.selectedList.items = this.selectedList.items.filter(
            i => i.id !== this.selectedItem.id
          );
          const listIndex = this.lists.findIndex(
            l => l.id === item.listId
          );
          this.selectedItem.listId = item.listId;
          this.lists[listIndex].items.push(this.selectedItem);
        }

        this.selectedItem.priority = item.priority;
        this.selectedItem.colour = item.colour;
        this.selectedItem.note = item.note;
        this.itemDetailsModalRef.hide();
        this.itemDetailsFormGroup.reset();
      },
      error => console.error(error)
    );
  }
  
  addItem() {
    const item = {
      id: 0,
      listId: this.selectedList.id,
      priority: this.priorityLevels[0].value,
      title: '',
      done: false
    } as TodoItemDto;

    this.selectedList.items.push(item);
    const index = this.selectedList.items.length - 1;
    this.editItem(item, 'itemTitle' + index);
  }

  editItem(item: TodoItemDto, inputId: string): void {
    this.selectedItem = item;
    setTimeout(() => document.getElementById(inputId).focus(), 100);
  }

  updateItem(item: TodoItemDto, pressedEnter: boolean = false): void {
    const isNewItem = item.id === 0;

    if (!item.title.trim()) {
      this.deleteItem(item);
      return;
    }

    if (item.id === 0) {
      this.itemsClient
        .create({
          ...item, listId: this.selectedList.id
        } as CreateTodoItemCommand)
        .subscribe(
          result => {
            item.id = result;
          },
          error => console.error(error)
        );
    } else {
      this.itemsClient.update(item.id, item).subscribe(
        () => console.log('Update succeeded.'),
        error => console.error(error)
      );
    }

    this.selectedItem = null;

    if (isNewItem && pressedEnter) {
      setTimeout(() => this.addItem(), 250);
    }
  }

  deleteItem(item: TodoItemDto, countDown?: boolean) {
    if (countDown) {
      if (this.deleting) {
        this.stopDeleteCountDown();
        return;
      }
      this.deleteCountDown = 3;
      this.deleting = true;
      this.deleteCountDownInterval = setInterval(() => {
        if (this.deleting && --this.deleteCountDown <= 0) {
          this.deleteItem(item, false);
        }
      }, 1000);
      return;
    }
    this.deleting = false;
    if (this.itemDetailsModalRef) {
      this.itemDetailsModalRef.hide();
    }

    if (item.id === 0) {
      const itemIndex = this.selectedList.items.indexOf(this.selectedItem);
      this.selectedList.items.splice(itemIndex, 1);
    } else {
      this.itemsClient.delete(item.id).subscribe(
        () =>
        (this.selectedList.items = this.selectedList.items.filter(
          t => t.id !== item.id
        )),
        error => console.error(error)
      );
    }
  }

  deleteTag(item: TodoItemTagDto, countDown?: boolean) {
    debugger;
    if (countDown) {
      if (this.deletingTag) {
        this.stopDeleteCountDown();
        return;
      }
      this.deleteCountDown = 3;
      this.deletingTag = true;
      this.deleteCountDownInterval = setInterval(() => {
        if (this.deletingTag && --this.deleteCountDown <= 0) {
          this.selectedItem;
          this.deleteTag(item, false);
        }
      }, 1000);
      return;
    }
    this.deletingTag = false;
    if (this.newTagModalRef) {
      this.newTagModalRef.hide();
    }

    this.itemTagsClient.delete(item.id).subscribe(
      () =>
      (
        this.selectedItem.tags.splice(this.selectedItem.tags.indexOf(item)+1,1)
      ),
      error => console.error(error)
    );
  }

  addItemTag(): void {
    const tag = this.itemDetailsTagFormGroup.value;

    this.itemTagsClient.create(tag as CreateTodoItemTagCommand).subscribe(
      result => {
        tag.id = result;
        this.selectedItem.tags.push(tag);
        this.newTagModalRef.hide();
        this.newTagsEditor = {};
        this.itemDetailsTagFormGroup.reset();
      },
      error => {
        const errors = JSON.parse(error.response);
        if (errors && errors.title) {
          this.newTagsEditor.error = errors.title[0];
        }

        setTimeout(() => document.getElementById('title').focus(), 250);
      }
    );
  }


  stopDeleteCountDown() {
    clearInterval(this.deleteCountDownInterval);
    this.deleteCountDown = 0;
    this.deleting = false;
    this.deletingTag = false;
  }

  applyFilter(event: Event) {
    this.lists = JSON.parse(JSON.stringify(this.unfilteredLists));

    const filterValue = ((event.target as HTMLInputElement).value).trim().toLowerCase();

    if (filterValue != null && filterValue != "") {
      this.lists.forEach(list => {
        list.items.forEach(item => {
          item.tags = item.tags.filter(tag => tag.title.trim().toLowerCase().includes(filterValue));
        });
        list.items = list.items.filter(item => item.tags.length > 0);
      });
      this.lists = this.lists.filter(list => list.items.length > 0);
    }
    this.selectedList = this.lists[0];
   }
}
