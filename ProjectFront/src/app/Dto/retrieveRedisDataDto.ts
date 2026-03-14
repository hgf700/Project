import { ObjectType } from "../enum/objectType";
import { UserActionType } from "../enum/userActionType";

export interface retrieveRedisDataDto {
  userNick: string;
  userCommittedAction: UserActionType;
  objectId: number;
  objectType: ObjectType;
  createdDate: Date;
}
