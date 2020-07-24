import React, { useContext } from 'react';
import { Modal } from 'semantic-ui-react';
import { RootsStoreContext } from '../../stores/rootStore';
import { observer } from 'mobx-react-lite';

const ModalContainer = () => {
  const rootStore = useContext(RootsStoreContext);
  const {
    modal: { open, body },
    closeModal,
  } = rootStore.modalStore;

  return (
    <Modal open={open} onClose={closeModal} size="mini">
      <Modal.Content>{body}</Modal.Content>
    </Modal>
  );
};

export default observer(ModalContainer);
