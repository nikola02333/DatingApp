import { DatePipe } from '@angular/common';
import { ToastrService, ToastrModule } from 'ngx-toastr';
import { UserService } from './../../_services/user.service';
import { Component, OnInit, ViewChild } from '@angular/core';
import { User } from 'src/app/_models/user';
import { ActivatedRoute } from '@angular/router';
import { NgxGalleryOptions, NgxGalleryImage, NgxGalleryAnimation } from 'ngx-gallery';
import { TabsetComponent } from 'ngx-bootstrap';

@Component({
  selector: 'app-member-detail',
  templateUrl: './member-detail.component.html',
  styleUrls: ['./member-detail.component.css']
})
export class MemberDetailComponent implements OnInit {
  @ViewChild('memberTabs' ,{ static: true}) memberTabs: TabsetComponent;
  user: User;
  galleryOptions: NgxGalleryOptions[];
  galleryImages: NgxGalleryImage[];
  constructor(private userService: UserService, private toast: ToastrService,
              private route: ActivatedRoute,
              public datepipe: DatePipe) { }
              // a ctivatedRoute nam treba da bi ivukli ID iz URl-a
  ngOnInit() {
     this.route.data.subscribe( data => {
       this.user = data['user'];
     });

     this.route.queryParams.subscribe(params => {
      const slectedTab = params['tab'];
      this.memberTabs.tabs[slectedTab > 0 ? slectedTab : 0].active = true;
    });
    
     this.galleryImages = this.getImages();
     this.galleryOptions = [
      {
       width: '500px',
       height: '500px',
       imagePercent: 100,
       thumbnailsColumns: 4,
       imageAnimation: NgxGalleryAnimation.Slide,
      preview: false,
     }
    ];
  }
  getImages() {
    const imageUrls = [];
    // tslint:disable-next-line:prefer-for-of
    for (let i = 0 ; i < this.user.photos.length ; i++) {
      imageUrls.push( {
        small: this.user.photos[i].url,
        medium: this.user.photos[i].url,
        big: this.user.photos[i].url,
        decription: this.user.photos[i].description
      });
    }
    return imageUrls;
  }
 /* loadUser() {
    this.userService.getUser(+this.route.snapshot.params.id).subscribe((user: User) => {
      this.user = user;
      console.log(user.username);
    }, error => {
      this.toast.error(error);
    });
}*/
  selectTab(tabId: number) {
    this.memberTabs.tabs[tabId].active = true;
  }
}
