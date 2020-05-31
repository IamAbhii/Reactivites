import React, { SyntheticEvent, useContext } from 'react';
import { Grid } from 'semantic-ui-react';
import { IActivity } from '../../../app/models/activity';
import ActivityList from './ActivityList';
import ActivityDetails from '../details/ActivityDetails';
import ActivityForm from '../form/ActivityForm';
import { observer } from 'mobx-react-lite';
import ActivityStore from '../../../app/stores/activityStore';

interface IProps {
  setEditMode: (editMode: boolean) => void;
  setSelectedActivity: (activity: IActivity | null) => void;
  editActivity: (activity: IActivity) => void;
  deleteActivity: (e: SyntheticEvent<HTMLButtonElement>, id: string) => void;
  submiting: boolean;
  target: string;
}

const ActivityDashboard: React.FC<IProps> = ({
  setEditMode,
  setSelectedActivity,
  editActivity,
  deleteActivity,
  submiting,
  target,
}) => {
  const activityStore = useContext(ActivityStore);
  const { editMode, selectedActivity } = activityStore;

  return (
    <Grid>
      <Grid.Column width={10}>
        <ActivityList
          submiting={submiting}
          deleteActivity={deleteActivity}
          target={target}
        />
      </Grid.Column>
      <Grid.Column width={6}>
        {/* here right to the && will be only excuted if left is not null */}
        {selectedActivity && !editMode && (
          <ActivityDetails
            setEditMode={setEditMode}
            setSelectedActivity={setSelectedActivity}
          />
        )}
        {editMode && (
          <ActivityForm
            submiting={submiting}
            key={(selectedActivity && selectedActivity.id) || 0}
            setEditMode={setEditMode}
            activity={selectedActivity!}
            editActivity={editActivity}
          />
        )}
      </Grid.Column>
    </Grid>
  );
};

export default observer(ActivityDashboard);
