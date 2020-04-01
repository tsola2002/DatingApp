import {Injectable} from '@angular/core';
import { CanDeactivate } from '@angular/router';
import { MemberEditComponent } from '../members/member-edit/member-edit.component';

@Injectable()
export class PreventUnsavedChanges implements CanDeactivate<MemberEditComponent> {
    canDeactivate(component: MemberEditComponent) {
        // if the edit form has been changed we'll pop-up the confirm dialog box
        if (component.editForm.dirty) {
            return confirm('Are you sure you want to continue?  Any unsaved changes will be lost');
        }
        return true;
    }
}
