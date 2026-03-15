import { RetrieveRedisData } from '../../Services/RetrieveRedisData';
import { Component, OnInit, Inject } from '@angular/core';
import { retrieveRedisDataDto } from '../../Dto/retrieveRedisDataDto';
import { CommonModule } from '@angular/common';
import { UserActionType } from '../../enum/userActionType';

@Component({
  selector: 'app-redis-retive-data',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './redis-retive-data.html',
  styleUrl: './redis-retive-data.css',
})
export class RedisRetiveData implements OnInit{
  redisdata: retrieveRedisDataDto[]=[];
  loading = false;  
  
  constructor(
    private retrieveRedis: RetrieveRedisData,
  ) {}

  ngOnInit(): void {
    this.loadRedisData();
  }

  loadRedisData() {
    this.loading = true;
    this.retrieveRedis.getRedisData().subscribe({
      next: (value) => {
        console.log(value); 
        this.redisdata = value.map(x => ({ ...x, createdDate: new Date(x.createdDate) }));
        this.loading = false;
      },
      error: (err) => {
        console.error(err);
        this.loading = false;
      },
    });
  }
}
