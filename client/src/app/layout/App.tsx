import React, { useState, useEffect } from 'react';
import { List } from 'semantic-ui-react';

import { IActivity } from '../models/Activity';
import NavBar from './../../features/nav/NavBar';
import agent from '../api/agent';

function App() {
  const [activities, setActivities] = useState<IActivity[]>([]);

  useEffect(() => {
    agent.Activities.list().then((response) => {
      setActivities(response);
    });
  }, []);

  return (
    <div>
      <NavBar />
      <List>
        {activities.map((activity) => (
          <List.Item key={activity.id}>{activity.title}</List.Item>
        ))}
      </List>
    </div>
  );
}

export default App;
