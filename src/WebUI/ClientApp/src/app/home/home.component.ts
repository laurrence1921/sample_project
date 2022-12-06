import { AfterViewInit, Component, OnInit } from '@angular/core';
import { Subject } from 'rxjs';
import { AccountsViewerClient, FileParameter } from '../web-api-client';
import { catchError, debounceTime, startWith, switchMap, tap, finalize } from "rxjs/operators";
import { AuthorizeService } from '../../api-authorization/authorize.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit, AfterViewInit {

  public searchObs$: Subject<string> = new Subject();
  accountList: any;

  constructor(private service: AccountsViewerClient,
    private authService: AuthorizeService) { }


  userRole: string = null;
  file: FileParameter;
  month = ["January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"];
  currentHeaderText: string;

  ngOnInit(): void {
    this.currentHeaderText = this.month[new Date().getMonth()] + " "+ new Date().getFullYear()
    this.searchObs$.pipe(
      startWith(""),
      debounceTime(300),
      switchMap(values => {
        return this.service.getBalances(this.month[new Date().getMonth()])
      })
    ).subscribe(res => {
      //console.log(res)
      this.accountList = res
    })

    if (this.authService.isLoggedIn) {
      let tmp = this.authService.getUserProfile()
      this.userRole = tmp.roleName
    }
  }

  ngAfterViewInit() {

  }

  performUpload(e) {
    let file = e.target.files[0]

    const reader = new FileReader();
    reader.onload = () => { };
    reader.readAsText(file);

    this.file = {
      data: file,
      fileName: file.name || 'attachment'
    };

    //console.log(e)
  }

  completeUpload() {
    //console.log('upload btn')
    this.service.uploadBalance(this.file).subscribe(res => {
      //console.log(res)
      if (res) {
        alert('File has been uploaded')
      }
    }, err => {
      //console.log('error')
      alert('Error uploading file')
    })
  }
}
